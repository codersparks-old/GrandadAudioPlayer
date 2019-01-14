using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Timers;
using GrandadAudioPlayer.Attributes;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Prism;
using log4net;
using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class AdminViewModel : ErrorCheckingBindableBase
    {

        private static readonly string AllowedExtensionsRegexString = @"^\.[A-Za-z0-9]+(?:,\.[A-Za-z0-9]+)*$";
        private static readonly Regex AllowedExtensionsRegex = new Regex(AllowedExtensionsRegexString, RegexOptions.Compiled);

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AdminViewModel));

        public static string Version => ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(),
                typeof(AssemblyFileVersionAttribute), false)
            ).Version;

        public static string BuildTag
        {
            get
            {
                string buildTag = null;
                Type propertyType = null;

                var assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    propertyType = assembly.EntryPoint.ReflectedType;
                }

                if (propertyType == null) return buildTag;

                var objects =
                    propertyType.Module.Assembly.GetCustomAttributes(typeof(BuildTagAttribute), false);

                if (objects.Length > 0)
                {
                    buildTag = ((BuildTagAttribute)objects[0]).BuildTag;
                }

                return buildTag;
            }
        }

        private readonly ConfigurationManager _configurationManager;

        public string FolderPath
        {
            get => _configurationManager.Configuration.FolderPath;
            set { _configurationManager.Configuration.FolderPath = value; SaveConfiguration(); RaisePropertyChanged("FolderPath"); }
        }

        public string AllowedExtensions
        {
            get => string.Join(",", _configurationManager.Configuration.AllowedExtensions);
            set
            {
                _configurationManager.Configuration.AllowedExtensions.Clear();

                foreach (var s in value.Split(','))
                {
                    _configurationManager.Configuration.AllowedExtensions.Add(s);
                }

                RaisePropertyChanged("AllowedExtensions");
            }
        }
        
        public bool ForceUpdate
        {
            get => _configurationManager.Configuration.AutoRestartOnUpdate;
            set
            {
                _configurationManager.Configuration.AutoRestartOnUpdate = value;
                SaveConfiguration();
                RaisePropertyChanged("ForceUpdate");
            }
        }

        public bool HasNoErrors => !HasErrors;

        private string _feedbackMessage;

        public string FeedbackMessage
        {
            get => _feedbackMessage;
            set => SetProperty(ref _feedbackMessage, value);
        }
        public DelegateCommand SaveConfigurationCommand { get; }
        public DelegateCommand LoadConfigurationCommand { get; }
        public DelegateCommand OpenFileDialogCommand { get; }
        public DelegateCommand CloseAdminViewCommand { get; }
        

        public AdminViewModel(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;

            SaveConfigurationCommand = new DelegateCommand(SaveConfiguration);
            LoadConfigurationCommand = new DelegateCommand(LoadConfiguration);
            OpenFileDialogCommand = new DelegateCommand(OpenFileDialogMethod);
            CloseAdminViewCommand = new DelegateCommand(CloseAdminViewMethod, CanCloseAdminViewMethod).ObservesProperty(() => HasErrors);
        }

        private void SaveConfiguration()
        {
            if (HasErrors)
            {
                Logger.Error("Cannot save configuration as there are errors");
                return;
            }

            _configurationManager.SaveConfiguration();

            FeedbackMessage = "Configuration Saved!";
            var t = new Timer { Interval = 5000 };


            t.Elapsed += (s, e) =>
            {
                FeedbackMessage = "";
                RaisePropertyChanged("FeedbackMessage");
                t.Stop();
            };

            t.Start();
        }

        private void LoadConfiguration()
        {
            _configurationManager.LoadConfiguration();
            RaisePropertyChanged("FolderPath");
            RaisePropertyChanged("AllowedExtensions");
        }

        private void OpenFileDialogMethod()
        {
            var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = Directory.Exists(FolderPath) ? FolderPath : @"C:/",
                IsFolderPicker = true
            };


            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPath = dialog.FileName;
            }
        }

        protected override void LocalValidation(ref Dictionary<string, List<string>> propertyErrors)
        {
            _validateAllowedExtensions(ref propertyErrors);
        }

        private void _validateAllowedExtensions(ref Dictionary<string, List<string>> propertyErrors)
        {

            const string propertyName = "AllowedExtensions";
            if (propertyErrors.TryGetValue(propertyName, out var allowedExtensionsErrorList) == false)
            {
                allowedExtensionsErrorList = new List<string>();
            }
            else
            {
                allowedExtensionsErrorList.Clear();
            }

            if (string.IsNullOrWhiteSpace(AllowedExtensions))
            {
                allowedExtensionsErrorList.Add("Allowed Extensions must not be empty");
            }

            if (!AllowedExtensionsRegex.IsMatch(AllowedExtensions))
            {
                allowedExtensionsErrorList.Add($"Allowed Extensions must be comma seperated list with no spaces (regex: {AllowedExtensionsRegexString} )");
            }

            propertyErrors[propertyName] = allowedExtensionsErrorList;
        }

        private void CloseAdminViewMethod()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);

        }

        private bool CanCloseAdminViewMethod()
        {
            if (!HasErrors) return true;


            FeedbackMessage = "Cannot close dialog until errors are resolved";
            return false;

        }

    }
}
