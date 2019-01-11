using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AudioSwitcher.AudioApi.CoreAudio;
using GrandadAudioPlayer.Model;
using GrandadAudioPlayer.Utils.Configuration;
using log4net;
using NAudio.Wave;
using NuGet;
using Prism.Mvvm;

namespace GrandadAudioPlayer.Utils.Playlist
{
    public class PlaylistManager : BindableBase, IDisposable, IPlaylistManager
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(PlaylistManager));

        private string _rootFolder;
        private LinkedListNode<PlaylistItem> _currentItem;

        private MediaFoundationReader _mediaFoundationReader;
        private WaveOut _waveOut;
        private float _volume = 0.5f;
        private readonly FileUtils _fileUtils;
        private readonly ConfigurationManager _configurationManager;

        private readonly CoreAudioDevice _defaultAudioDevice = new CoreAudioController().DefaultPlaybackDevice;

        public PlaylistManager(FileUtils fileUtils, ConfigurationManager configurationManager)
        {
            _fileUtils = fileUtils;
            _configurationManager = configurationManager;
            _rootFolder = _configurationManager.Configuration.FolderPath;
            ReloadPlaylist();
        }

        public string RootFolder
        {
            get => _rootFolder;
            set
            {
                if (_rootFolder == value) return;
                _rootFolder = value;
                Logger.Debug($"Root folder set to {_rootFolder}");
                ReloadPlaylist();
            }
        }

        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get => _isPlaying;
            private set
            {
                SetProperty(ref _isPlaying, value, "IsPlaying"); 
                Logger.Debug($"_isPlaying: {_isPlaying}");
            }
        }

        private bool _isPaused;
        public bool IsPaused
        {
            get => _isPaused;
            private set => SetProperty(ref _isPaused, value, "IsPaused");
        }

        public PlaylistItem CurrentItem
        {
            get =>_currentItem?.Value;
            set
            {

                if (value == null) return;

                var newItem = value;

                Logger.Debug("Updating current item..." + newItem.Path);

                if (_currentItem.Value == newItem) return;


                SetProperty(ref _currentItem, _playlist.Find(newItem));

                if (!IsPlaying) return;

                Stop();
                Play();
            }
        }


        private readonly LinkedList<PlaylistItem> _playlist = new LinkedList<PlaylistItem>();

        public ObservableCollection<PlaylistItem> Playlist { get; private set; }

        public string CurrentPosition => _mediaFoundationReader != null ? _mediaFoundationReader.CurrentTime.ToString(@"mm\:ss") : TimeSpan.Zero.ToString(@"mm\:ss");

        public double CurrentPositionPercentage
        {
            get
            {
                if (_waveOut != null && _mediaFoundationReader != null)
                {
                    var position = _waveOut.GetPosition();
                    var length = _mediaFoundationReader.Length;
                    var positionPercentage = (double)_waveOut.GetPosition() / _mediaFoundationReader.Length * 100;
                    return positionPercentage;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Volume
        {
            get => (int)_defaultAudioDevice.Volume;
            set { _defaultAudioDevice.Volume = value; RaisePropertyChanged("Volume"); }
        }

        public void ReloadPlaylist()
        {

            Logger.Debug("Reloading playlist");
            _playlist.Clear();

            _playlist.AddRange(_fileUtils.GetPlaylistItemsUnderDirectory(_rootFolder, _configurationManager.Configuration.AllowedExtensions));

            _currentItem = _playlist.First;
            RaisePropertyChanged("CurrentItem");
            Playlist = new ObservableCollection<PlaylistItem>(_playlist);
            Logger.Debug($"Current item set to {_currentItem?.Value.Name}");

        }


        public void Stop()
        {

            if (_currentItem == null) return;

            Logger.Info("Stopping playback...");
            if (_waveOut != null)
            {
                Logger.Debug("Disposing of wave out and mp3FileReader");
                _waveOut.Dispose();
                _waveOut = null;
                _mediaFoundationReader.Dispose();
                _mediaFoundationReader = null;
            }
            IsPlaying = false;
            IsPaused = false;
            Logger.Debug("IsPlaying set to: " + IsPlaying);
        }

        public void Play()
        {
            Logger.Debug("Play() called");
            if (_currentItem == null) return;

            if (_waveOut == null)
            {
                _initialisePlayer();
            }

            _waveOut.Play();
            IsPlaying = true;
            IsPaused = false;

        }

        public void Pause()
        {
            Logger.Debug("Pause() called");
            if (_currentItem == null) return;

            _waveOut?.Pause();
            IsPaused = true;
        }

        public void NextTrack()
        {
            Logger.Debug("NextTrack() called");
            if (_currentItem == null) return;

            // We keep track if it was playing or paused ready for later
            var wasPlaying = IsPlaying;
            var wasPaused = IsPaused;
            Logger.Debug($"Was paused: {wasPaused} and Was playing: {wasPlaying}");

            // We now stop if it is playing
            if (IsPlaying)
            {
                Stop();
            }

            // Move to the next track (or first if at the end)
            var next = _currentItem.Next;

            _currentItem = next ?? _playlist.First;
            RaisePropertyChanged("CurrentItem");

            // If it was playing originally then we play again
            if (!wasPlaying) return;

            Play();

            if (wasPaused)
            {
                Pause();
            }
        }

        public void PreviousTrack()
        {

            Logger.Debug("PreviousTrack() called");
            if (_currentItem == null) return;

            // We keep track if it was playing ready for later
            var wasPlaying = IsPlaying;
            var wasPaused = IsPaused;
            Logger.Debug($"Was paused: {wasPaused} and Was playing: {wasPlaying}");

            // We now stop if it is playing
            if (IsPlaying)
            {
                Stop();
            }

            // Move to the previous track (or last if at the end)
            var previous = _currentItem.Previous;

            _currentItem = previous ?? _playlist.Last;
            RaisePropertyChanged("CurrentItem");

            // If it was playing originally then we play again
            if (!wasPlaying) return;

            Play();

            if (wasPaused)
            {
                Pause();
            }
        }

        private void _initialisePlayer()
        {
            Logger.Debug("Initialising player with file: " + CurrentItem.Path);
            _mediaFoundationReader = new MediaFoundationReader(CurrentItem.Path);
            _waveOut = new WaveOut();
            _waveOut.PlaybackStopped += OnPlaybackStopped;
            _waveOut.Volume = _volume;
            _waveOut.Init(_mediaFoundationReader);
        }

        private void OnPlaybackStopped(object obj, StoppedEventArgs args)
        {

            if (IsPlaying)
            {
                NextTrack();
            }
        }


        public void Dispose()
        {
            _mediaFoundationReader?.Dispose();
            _waveOut?.Dispose();
        }

    }
}
