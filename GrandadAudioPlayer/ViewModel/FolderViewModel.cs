using GalaSoft.MvvmLight;
using GrandadAudioPlayer.Utils;
using GrandadAudioPlayerClassLibrary.Configuration;
using System.Collections.ObjectModel;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Messaging;
using GrandadAudioPlayer.Model.FolderView;
using GrandadAudioPlayer.Model.PlayList;

namespace GrandadAudioPlayer.ViewModel
{
    public class FolderViewModel : ViewModelBase
    {

        private FolderItemBase _selectedItem = null;
        public FolderItemBase SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;

                this._selectedItem = value;
                RaisePropertyChanged("SelectedItem");

                if (_selectedItem == null) return;

                var messageBody = new PlaylistMessage(_selectedItem.Path);
                Messenger.Default.Send<NotificationMessage<PlaylistMessage>>(new NotificationMessage<PlaylistMessage>(messageBody, "Playlist Updated"));
            }
        } 

        public FolderViewModel()
        {
            this.RootFolder = new ObservableCollection<FolderItemBase>(
                FolderUtils.GetTreeStructure(ConfigurationManager.Instance.Configuration.FolderPath));

        }

        public ObservableCollection<FolderItemBase> RootFolder { get; }

    }
}
