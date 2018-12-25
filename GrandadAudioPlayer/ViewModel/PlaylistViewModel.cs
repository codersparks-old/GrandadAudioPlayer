using System.Collections.ObjectModel;
using System.Timers;
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

        private Timer _positionUpdateTimer = null;

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

        private string _position;

        public string Position
        {
            get => _position;
            private set
            {
                _position = value;
                RaisePropertyChanged("Position");
            }
        }

        private double _positionPercentage;

        public double PositionPercentage
        {
            get => _positionPercentage;
            private set
            {
                _positionPercentage = value;
                RaisePropertyChanged("PositionPercentage");
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
            _startPositionTimer();
        }

        public bool CanPlayMethod()
        {
            return _currentItem != null && (!_playlistManager.IsPlaying || _playlistManager.IsPaused);
        }

        public void PauseMethod()
        {
            _playlistManager.Pause();
            _stopPositionTimer();
        }

        public bool CanPauseMethod()
        {
            return _currentItem != null && (_playlistManager.IsPlaying && !_playlistManager.IsPaused);
        }

        public void StopMethod()
        {
            _playlistManager.Stop();
            _stopPositionTimer();
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

        private void _startPositionTimer()
        {
            _positionUpdateTimer = new Timer {Interval = 100};
            _positionUpdateTimer.Elapsed += new ElapsedEventHandler(_updatePosition);

            _positionUpdateTimer.Start();

        }

        private void _stopPositionTimer()
        {
            _positionUpdateTimer.Stop();
            _positionUpdateTimer = null;
        }

        private void _updatePosition(object sender, ElapsedEventArgs e)
        {
            Position = _playlistManager.CurrentPosition;
            PositionPercentage = _playlistManager.CurrentPositionPercentage;
        }
    }

    
}