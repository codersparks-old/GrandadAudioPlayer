using System.IO;
using System.Timers;
using GrandadAudioPlayer.Utils.Configuration;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class AdminViewModel : BindableBase
    {

        private readonly ConfigurationManager _configurationManager;

        public string FolderPath
        {
            get => _configurationManager.Configuration.FolderPath;
            set { _configurationManager.Configuration.FolderPath = value; SaveConfiguration(); RaisePropertyChanged("FolderPath");}
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

        private string _feedbackMessage;

        public string FeedbackMessage
        {
            get => _feedbackMessage;
            set => SetProperty(ref _feedbackMessage, value);
        }
        public DelegateCommand SaveConfigurationCommand { get; }
        public DelegateCommand LoadConfigurationCommand { get; }
        public DelegateCommand OpenFileDialogCommand { get; }

        public AdminViewModel(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;

            SaveConfigurationCommand = new DelegateCommand(SaveConfiguration);
            LoadConfigurationCommand = new DelegateCommand(LoadConfiguration);
            OpenFileDialogCommand = new DelegateCommand(OpenFileDialogMethod);
        }

        private void SaveConfiguration()
        {
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
    }
}
