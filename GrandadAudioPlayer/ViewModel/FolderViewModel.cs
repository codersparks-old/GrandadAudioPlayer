using GalaSoft.MvvmLight;
using GrandadAudioPlayer.Model.TreeView;
using GrandadAudioPlayer.Utils;
using GrandadAudioPlayerClassLibrary.Configuration;
using System.Collections.ObjectModel;
using CommonServiceLocator;

namespace GrandadAudioPlayer.ViewModel
{
    public class FolderViewModel : ViewModelBase
    {

        public FolderViewModel()
        {
            this.RootFolder = new ObservableCollection<TreeViewBase>(
                FolderTreeBuilder.getTreeStructure(ConfigurationManager.Instance.Configuration.FolderPath));

        }

        public ObservableCollection<TreeViewBase> RootFolder { get; }

    }
}
