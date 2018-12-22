using GalaSoft.MvvmLight;
using GrandadAudioPlayer.Utils;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using GrandadAudioPlayer.Model.FolderView;
using GrandadAudioPlayer.Model.PlayList;
using GrandadAudioPlayer.Utils.Configuration;

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
            this.ReloadFolderStructure();
            Messenger.Default.Register < NotificationMessage<FolderMessage>>(this,  RecieveFolderMessage);
        }

        public void ReloadFolderStructure()
        {
            this.RootFolder = new ObservableCollection<FolderItemBase>(FolderUtils.GetTreeStructure(ConfigurationManager.Instance.Configuration.FolderPath));
            RaisePropertyChanged("RootFolder");
        }

        private void RecieveFolderMessage(NotificationMessage<FolderMessage> message)
        {
            this.ReloadFolderStructure();
        }

        public ObservableCollection<FolderItemBase> RootFolder { get; private set; }

    }
}
