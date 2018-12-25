using System.Collections.ObjectModel;
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

        public ICommand PlayCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand PauseCommand { get; }

        private PlaylistItem _currentItem;
        public PlaylistItem CurrentItem
        {
            get => _currentItem;
            set
            {
                _currentItem = value;
                RaisePropertyChanged("CurrentItem");
                _playlistManager.CurrentItem = _currentItem;
            }
        }

        public int Volume
        {
            get => _playlistManager.Volume;
            set
            {
                if (_playlistManager != null) _playlistManager.Volume = value;
            }
        }


        public PlaylistViewModel()
        {
            Messenger.Default.Register<NotificationMessage<PlaylistMessage>>(this, PlaylistUpdate );
            PlayCommand = new RelayCommand(PlayMethod, CanPlayMethod);
            StopCommand = new RelayCommand(StopMethod, CanStopMethod);
            NextCommand = new RelayCommand(NextMethod, CanNextMethod);
            PreviousCommand = new RelayCommand(PreviousMethod, CanPreviousMethod);
            PauseCommand = new RelayCommand(PauseMethod, CanPauseMethod);

            _playlistManager.OnTrackChanged += OnTrackUpdated;

        }

        public void OnTrackUpdated(object source, PlaylistEventArgs e)
        {
            CurrentItem = e.PlaylistItem;
        }

        public void PlaylistUpdate(NotificationMessage<PlaylistMessage> message)
        {
            _playlistManager.Stop();
            _playlistManager.RootFolder = message.Content.Path;
            _playlistManager.ReloadPlaylist();
            Playlist = new ObservableCollection<PlaylistItem>(_playlistManager.Playlist);
            RaisePropertyChanged("Playlist");
        }

        public void PlayMethod()
        {
            _playlistManager.Play();
        }

        public bool CanPlayMethod()
        {
            return _currentItem != null && (!_playlistManager.IsPlaying || _playlistManager.IsPaused);
        }

        public void PauseMethod()
        {
            _playlistManager.Pause();
        }

        public bool CanPauseMethod()
        {
            return _currentItem != null && (_playlistManager.IsPlaying && !_playlistManager.IsPaused);
        }

        public void StopMethod()
        {
            _playlistManager.Stop();
        }

        public bool CanStopMethod()
        {
            return _currentItem != null && _playlistManager.IsPlaying;
        }

        public void NextMethod()
        {
            _playlistManager.NextTrack();
        }

        public bool CanNextMethod()
        {
            return _currentItem != null;
        }

        public void PreviousMethod()
        {
            _playlistManager.PreviousTrack();
        }

        public bool CanPreviousMethod()
        {
            return _currentItem != null;
        }
    }

    
}