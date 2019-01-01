using AudioSwitcher.AudioApi.CoreAudio;
using GrandadAudioPlayer.Utils;
using GrandadAudioPlayer.Utils.Configuration;
using log4net;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

namespace GrandadAudioPlayer.Model.PlayList
{
    public delegate void TrackChangedEventHandler(object source, PlaylistEventArgs e);

    public class PlaylistEventArgs : EventArgs
    {
        public PlaylistEventArgs(PlaylistItem playlistItem)
        {
            PlaylistItem = playlistItem;
        }

        public PlaylistItem PlaylistItem { get; }
    }

    public sealed class PlaylistManager : IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PlaylistManager));

        //        private static readonly Lazy<PlaylistManager> LazyInstance =

        //            new Lazy<PlaylistManager>(() => new PlaylistManager());

        //

        //        public static PlaylistManager Instance => LazyInstance.Value;
        private string _rootFolder;

        private LinkedListNode<PlaylistItem> _currentItem;

        private readonly ConfigurationManager _configurationManager;

        private MediaFoundationReader _mp3FileReader;

        private WaveOut _waveOut;

        private float _volume = 0.5f;

        private readonly CoreAudioDevice _defaultAudioDevice = new CoreAudioController().DefaultPlaybackDevice;

        public PlaylistManager(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public string RootFolder
        {
            get => _rootFolder;
            set
            {
                if (_rootFolder == value) return;
                _rootFolder = value;
                ReloadPlaylist();
            }
        }

        public bool IsPlaying { get; private set; }

        public bool IsPaused { get; private set; }

        public PlaylistItem CurrentItem
        {
            get => _currentItem?.Value;
            set
            {

                if (value == null) return;

                var newItem = value;

                Logger.Debug("Updating current item..." + newItem.Path);

                if (_currentItem.Value != newItem)
                {
                    Logger.Debug("New value != currentItem value");
                    _currentItem = Playlist.Find(newItem);
                    OnTrackChanged?.Invoke(this, new PlaylistEventArgs(newItem));

                    if (IsPlaying)
                    {
                        Stop();
                        Play();
                    }
                }
            }
        }

        public LinkedList<PlaylistItem> Playlist { get; } = new LinkedList<PlaylistItem>();

        public string CurrentPosition => _mp3FileReader != null ? _mp3FileReader.CurrentTime.ToString(@"mm\:ss") : TimeSpan.Zero.ToString();

        public double CurrentPositionPercentage
        {
            get
            {
                if (_waveOut != null && _mp3FileReader != null)
                {
                    long position = _waveOut.GetPosition();
                    long length = _mp3FileReader.Length;
                    double positionPercentage = (double)_waveOut.GetPosition() / _mp3FileReader.Length * 100;
                    return positionPercentage;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Volume { get => (int)_defaultAudioDevice.Volume; set => _defaultAudioDevice.Volume = value; }

        public event TrackChangedEventHandler OnTrackChanged;

        public void ReloadPlaylist()
        {

            Logger.Debug("Reloading playlist");
            Playlist.Clear();

            var files = FolderUtils.GetFilesUnderFolder(_rootFolder);

            if (files.Count == 0) return;

            foreach (var f in files)
            {
                if (!_configurationManager.Configuration.AllowedExtensions.Contains(Path.GetExtension(f.Path))) continue;
                var playlistItem = new PlaylistItem(f.Path);
                Playlist.AddLast(playlistItem);
            }

            _currentItem = Playlist.First;
            Logger.Debug("Current item set to " + _currentItem?.Value.Name);


            OnTrackChanged?.Invoke(this, new PlaylistEventArgs(CurrentItem));
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
                _mp3FileReader.Dispose();
                _mp3FileReader = null;
            }
            IsPlaying = false;
            IsPaused = false;
            Logger.Debug("IsPlaying set to: " + IsPlaying);
        }

        public void Play()
        {
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
            if (_currentItem == null) return;

            _waveOut?.Pause();
            IsPaused = true;
        }

        public void NextTrack()
        {
            if (_currentItem == null) return;

            // We keep track if it was playing ready for later
            var wasPlaying = IsPlaying;
            var wasPaused = IsPaused;

            // We now stop if it is playing
            if (IsPlaying)
            {
                Stop();
            }

            // Move to the next track (or first if at the end)
            var next = _currentItem.Next;

            _currentItem = next ?? Playlist.First;

            OnTrackChanged?.Invoke(this, new PlaylistEventArgs(CurrentItem));

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

            if (_currentItem == null) return;

            // We keep track if it was playing ready for later
            var wasPlaying = IsPlaying;
            var wasPaused = IsPaused;

            // We now stop if it is playing
            if (IsPlaying)
            {
                Stop();
            }

            // Move to the previous track (or last if at the end)
            var previous = _currentItem.Previous;

            _currentItem = previous ?? Playlist.Last;

            OnTrackChanged?.Invoke(this, new PlaylistEventArgs(CurrentItem));


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
            _mp3FileReader = new MediaFoundationReader(CurrentItem.Path);
            _waveOut = new WaveOut();
            _waveOut.PlaybackStopped += OnPlaybackStopped;
            _waveOut.Volume = _volume;
            _waveOut.Init(_mp3FileReader);
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
            _mp3FileReader?.Dispose();
            _waveOut?.Dispose();
        }
    }
}
