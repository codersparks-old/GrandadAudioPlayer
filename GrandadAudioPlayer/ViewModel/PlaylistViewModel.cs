

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GrandadAudioPlayer.Model.PlayList;

namespace GrandadAudioPlayer.ViewModel
{
    public class PlaylistViewModel : ViewModelBase
    {

        private readonly PlaylistManager _playlistManager = PlaylistManager.Instance;

        public ObservableCollection<PlaylistItem> Playlist { get; set; }

        public ICommand PlayPauseCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }

        

        public PlaylistViewModel()
        {
            Messenger.Default.Register<NotificationMessage<PlaylistMessage>>(this, PlaylistUpdate );
            this.PlayPauseCommand = new RelayCommand(PlayPauseMethod);
            this.StopCommand = new RelayCommand(StopMethod);
            this.NextCommand = new RelayCommand(NextMethod);
            this.PreviousCommand = new RelayCommand(PreviousMethod);
            
        }

        public void PlaylistUpdate(NotificationMessage<PlaylistMessage> message)
        {
            this._playlistManager.RootFolder = message.Content.Path;
            this._playlistManager.ReloadPlaylist();
            this.Playlist = new ObservableCollection<PlaylistItem>(this._playlistManager.Playlist);
            RaisePropertyChanged("Playlist");
        }

        public void PlayPauseMethod()
        {
            this._playlistManager.PlayPause();
        }

        public void StopMethod()
        {
            this._playlistManager.Stop();
        }

        public void NextMethod()
        {
            this._playlistManager.NextTrack();
        }

        public void PreviousMethod()
        {
            this._playlistManager.PreviousTrack();
        }
    }

    
}