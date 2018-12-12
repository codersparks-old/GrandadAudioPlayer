using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GrandadAudioPlayer.Model.TreeView;
using GrandadAudioPlayer.Utils;
using GrandadAudioPlayerClassLibrary.Configuration;

namespace GrandadAudioPlayer.ViewModel
{
    public class FolderViewModel : ViewBase
    {
        public FolderViewModel()
        {
            this.RootFolder = new ObservableCollection<TreeViewBase>(
                FolderTreeBuilder.getTreeStructure(ConfigurationManager.Instance.Configuration.FolderPath));
        }

        public ObservableCollection<TreeViewBase> RootFolder { get; }
    }
}
