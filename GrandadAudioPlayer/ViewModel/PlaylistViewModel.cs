

using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GrandadAudioPlayer.Model.PlayList;

namespace GrandadAudioPlayer.ViewModel
{
    public class PlaylistViewModel : ViewModelBase
    {

        private readonly PlaylistManager _playlistManager = PlaylistManager.Instance;

        public ObservableCollection<PlaylistItem> Playlist { get; set; }

        public PlaylistViewModel()
        {
            Messenger.Default.Register<NotificationMessage<PlaylistMessage>>(this, PlaylistUpdate );
        }

        public void PlaylistUpdate(NotificationMessage<PlaylistMessage> message)
        {
            this._playlistManager.RootFolder = message.Content.Path;
            this._playlistManager.ReloadPlaylist();
            this.Playlist = new ObservableCollection<PlaylistItem>(this._playlistManager.Playlist);
            RaisePropertyChanged("Playlist");
        }
    }

    
}