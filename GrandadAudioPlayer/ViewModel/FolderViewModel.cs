using GalaSoft.MvvmLight;
using GrandadAudioPlayer.Utils;
using GrandadAudioPlayerClassLibrary.Configuration;
using System.Collections.ObjectModel;
using CommonServiceLocator;
using GrandadAudioPlayer.Model.FolderView;

namespace GrandadAudioPlayer.ViewModel
{
    public class FolderViewModel : ViewModelBase
    {

        public FolderViewModel()
        {
            this.RootFolder = new ObservableCollection<FolderItemBase>(
                FolderUtils.GetTreeStructure(ConfigurationManager.Instance.Configuration.FolderPath));

        }

        public ObservableCollection<FolderItemBase> RootFolder { get; }

    }
}
