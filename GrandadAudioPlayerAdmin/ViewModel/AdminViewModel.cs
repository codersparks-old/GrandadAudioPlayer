using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GrandadAudioPlayerClassLibrary.Configuration;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace GrandadAudioPlayerAdmin.ViewModel
{
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
    public class MainViewModel : ViewModelBase
    {

        private ConfigurationModel _configuration;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            this.LoadConfigurationMethod();

            LoadConfigurationCommand = new RelayCommand(LoadConfigurationMethod);
            SaveConfigurationCommand = new RelayCommand(SaveConfigurationMethod);
            OpenFileDialogCommand = new RelayCommand(OpenFileDialogMethod);
        }

        

        public string FolderPath
        {
            get { return _configuration.FolderPath; }
            set
            {
                _configuration.FolderPath = value;
                RaisePropertyChanged("FolderPath");
            }
        }

        public string AllowedExtensions
        {
            get
            {
                return string.Join(",", _configuration.AllowedExtensions);
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    _configuration.AllowedExtensions.Clear();
                    _configuration.AllowedExtensions.Add(".mp3");
                }
                else
                {
                    _configuration.AllowedExtensions.Clear();
                    _configuration.AllowedExtensions.AddRange(value.Split(','));
                }

                RaisePropertyChanged("AllowedExtenstions");
            }
        }

        public ICommand LoadConfigurationCommand { get; private set; }
        public ICommand SaveConfigurationCommand { get; private set; }
        public ICommand OpenFileDialogCommand { get; private set; }

        public string FeedbackMessage { get; set; }

        public void SaveConfigurationMethod()
        {
            ConfigurationManager.Instance.SaveConfiguration(this._configuration);
            this.FeedbackMessage = "Configuration saved!";
            RaisePropertyChanged("FeedbackMessage");

            var t = new Timer();

            t.Interval = 5000;

            t.Elapsed += (s, e) =>
            {
                this.FeedbackMessage = "";
                RaisePropertyChanged("FeedbackMessage");
                t.Stop();
            };

            t.Start();

        }

        public void LoadConfigurationMethod()
        {
            this._configuration = ConfigurationManager.Instance.Configuration;
            this.FolderPath = this._configuration.FolderPath;
        }

        private void OpenFileDialogMethod()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            if (Directory.Exists(FolderPath))
            {
                dialog.InitialDirectory = FolderPath;
            }
            else
            {
                dialog.InitialDirectory = @"C:/";
            }

            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FolderPath = dialog.FileName;
            }
        }

    }
}