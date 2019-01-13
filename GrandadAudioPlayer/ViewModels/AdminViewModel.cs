using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Prism;
using log4net;
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

        private string _feedbackMessage;

        public string FeedbackMessage
        {
            get => _feedbackMessage;
            set => SetProperty(ref _feedbackMessage, value);
        }
        public DelegateCommand SaveConfigurationCommand { get; }
        public DelegateCommand LoadConfigurationCommand { get; }
        public DelegateCommand OpenFileDialogCommand { get; }

        // TODO: Add way to close dialog through command (and therefore allow check of has errors)

        public AdminViewModel(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;

            SaveConfigurationCommand = new DelegateCommand(SaveConfiguration);
            LoadConfigurationCommand = new DelegateCommand(LoadConfiguration);
            OpenFileDialogCommand = new DelegateCommand(OpenFileDialogMethod);
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

    }
}
