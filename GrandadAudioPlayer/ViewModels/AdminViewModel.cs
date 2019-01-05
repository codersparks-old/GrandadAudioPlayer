using GrandadAudioPlayer.Utils.Configuration;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class AdminViewModel : BindableBase
    {

        private readonly ConfigurationManager _configurationManager;

        public string FolderPath
        {
            get => _configurationManager.Configuration.FolderPath;
            set => _configurationManager.Configuration.FolderPath = value;
        }

        public AdminViewModel(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }
    }
}
