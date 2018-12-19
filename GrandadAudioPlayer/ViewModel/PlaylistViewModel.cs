

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

        public ICommand PlayCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }

        private PlaylistItem _currentItem = null;
        public PlaylistItem CurrentItem
        {
            get => _currentItem;
            set
            {
                this._currentItem = value;
                RaisePropertyChanged("CurrentItem");
                this._playlistManager.CurrentItem = this._currentItem;
            }
        }

        public bool PlayButtonEnabled => !this._playlistManager.IsPlaying || this._playlistManager.IsPaused;
        public bool PauseButtonEnabled => !this._playlistManager.IsPaused;



        public PlaylistViewModel()
        {
            Messenger.Default.Register<NotificationMessage<PlaylistMessage>>(this, PlaylistUpdate );
            this.PlayCommand = new RelayCommand(PlayMethod, CanPlayMethod);
            this.StopCommand = new RelayCommand(StopMethod);
            this.NextCommand = new RelayCommand(NextMethod);
            this.PreviousCommand = new RelayCommand(PreviousMethod);
            this.PauseCommand = new RelayCommand(PauseMethod, CanPauseMethod);

            this._playlistManager.OnTrackChanged += this.OnTrackUpdated;

        }

        public void OnTrackUpdated(object source, PlaylistEventArgs e)
        {
            this.CurrentItem = e.PlaylistItem;
        }

        public void PlaylistUpdate(NotificationMessage<PlaylistMessage> message)
        {
            this._playlistManager.Stop();
            this._playlistManager.RootFolder = message.Content.Path;
            this._playlistManager.ReloadPlaylist();
            this.Playlist = new ObservableCollection<PlaylistItem>(this._playlistManager.Playlist);
            RaisePropertyChanged("Playlist");
        }

        public void PlayMethod()
        {
            this._playlistManager.Play();
        }

        public bool CanPlayMethod()
        {
            return !this._playlistManager.IsPlaying || this._playlistManager.IsPaused;
        }

        public void PauseMethod()
        {
            this._playlistManager.Pause();
        }

        public bool CanPauseMethod()
        {
            return this._playlistManager.IsPlaying && !this._playlistManager.IsPaused;
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