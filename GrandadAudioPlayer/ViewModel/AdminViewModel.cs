using System;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GrandadAudioPlayer.Model.Attributes;
using GrandadAudioPlayer.Utils.Configuration;
using Microsoft.WindowsAPICodePack.Dialogs;
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable ExplicitCallerInfoArgument

namespace GrandadAudioPlayer.ViewModel
{
    /// <inheritdoc/>
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AdminViewModel
        : ViewModelBase
    {

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

                Assembly assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    propertyType = assembly.EntryPoint.ReflectedType;
                }

                if (propertyType != null)
                {
                    object[] objects =
                        propertyType.Module.Assembly.GetCustomAttributes(typeof(BuildTagAttribute), false);

                    if (objects != null && objects.Length > 0)
                    {
                        buildTag = ((BuildTagAttribute) objects[0]).BuildTag;
                    } 
                }

                return buildTag;
            }
        }

        private ConfigurationModel _configuration;
        /// <inheritdoc />
        /// <summary>
        /// Initialises a new instance of the MainViewModel class.
        /// </summary>
        public AdminViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            LoadConfigurationMethod();

            LoadConfigurationCommand = new RelayCommand(LoadConfigurationMethod);
            SaveConfigurationCommand = new RelayCommand(SaveConfigurationMethod);
            OpenFileDialogCommand = new RelayCommand(OpenFileDialogMethod);
        }

        

        public string FolderPath
        {
            get => _configuration.FolderPath;
            set
            {
                _configuration.FolderPath = value;
                RaisePropertyChanged("FolderPath");
            }
        }

        public string AllowedExtensions
        {
            get => string.Join(",", _configuration.AllowedExtensions);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _configuration.AllowedExtensions.Clear();
                    _configuration.AllowedExtensions.Add(".mp3");
                    _configuration.AllowedExtensions.Add(".m4a");
                }
                else
                {
                    _configuration.AllowedExtensions.Clear();

                    foreach (var s in value.Split(','))
                    {
                        _configuration.AllowedExtensions.Add(s);
                    }
                }

                RaisePropertyChanged("AllowedExtensions");
            }
        }

        public ICommand LoadConfigurationCommand { get; }
        public ICommand SaveConfigurationCommand { get; }
        public ICommand OpenFileDialogCommand { get; }

        public string FeedbackMessage { get; set; }

        public void SaveConfigurationMethod()
        {
            ConfigurationManager.Instance.SaveConfiguration(_configuration);
            FeedbackMessage = "Configuration saved!";
            RaisePropertyChanged("FeedbackMessage");

            var t = new Timer {Interval = 5000};


            t.Elapsed += (s, e) =>
            {
                FeedbackMessage = "";
                RaisePropertyChanged("FeedbackMessage");
                t.Stop();
            };

            t.Start();

        }

        public void LoadConfigurationMethod()
        {
            _configuration = ConfigurationManager.Instance.Configuration;
            FolderPath = _configuration.FolderPath;
        }

        private void OpenFileDialogMethod()
        {
            var dialog = new CommonOpenFileDialog
            {
                InitialDirectory = Directory.Exists(FolderPath) ? FolderPath : @"C:/", IsFolderPicker = true
            };


            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPath = dialog.FileName;
            }
        }

    }
}