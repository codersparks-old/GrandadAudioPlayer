using System;
using System.Collections.Generic;
using System.IO;
using GrandadAudioPlayer.Utils;
using GrandadAudioPlayer.Utils.Configuration;
using log4net;
using NAudio.Wave;

namespace GrandadAudioPlayer.Model.PlayList
{
    public delegate void TrackChangedEventHandler(object source, PlaylistEventArgs e);

    public class PlaylistEventArgs : EventArgs
    {

        public PlaylistEventArgs(PlaylistItem playlistItem)
        {
            this.PlaylistItem = playlistItem;
        }

        public PlaylistItem PlaylistItem { get; private set; }
    }
    public sealed class PlaylistManager : IDisposable
    {
        private static ILog logger = LogManager.GetLogger(typeof(PlaylistManager));

        private static readonly HashSet<string> AllowedExtensions = ConfigurationManager.Instance.Configuration.AllowedExtensions;

        private static readonly Lazy<PlaylistManager> _lazyInstance =
            new Lazy<PlaylistManager>(() => new PlaylistManager());

        public static PlaylistManager Instance => _lazyInstance.Value;

        private string _rootFolder;
        private LinkedListNode<PlaylistItem> _currentItem = null;

        private MediaFoundationReader _mp3FileReader;
        private WaveOut _waveOut = null;

        public string RootFolder
        {
            get => _rootFolder;
            set
            {
                if (this._rootFolder == value) return;
                this._rootFolder = value;
                this.ReloadPlaylist();
            }
        }

        public bool IsPlaying { get; private set; } = false;
        public bool IsPaused { get; private set; } = false;

        public PlaylistItem CurrentItem
        {
            get => this._currentItem?.Value;
            set
            {

                if (value == null) return;

                var newItem = value;

                logger.Debug("Updating current item..." + newItem.Path);

                if (_currentItem.Value != newItem)
                {
                    logger.Debug("New value != currentItem value");
                    this._currentItem = this.Playlist.Find(newItem);
                    this.OnTrackChanged?.Invoke(this, new PlaylistEventArgs(newItem));

                    if (this.IsPlaying)
                    {
                        this.Stop();
                        this.Play();
                    }
                }
            }
        }

        public LinkedList<PlaylistItem> Playlist { get; } = new LinkedList<PlaylistItem>();

        public event TrackChangedEventHandler OnTrackChanged;

        public void ReloadPlaylist()
        {

            logger.Debug("Reloading playlist");
            this.Playlist.Clear();

            var files = FolderUtils.GetFilesUnderFolder(this._rootFolder);

            if (files.Count == 0) return;

            foreach (var f in files)
            {
                if (!AllowedExtensions.Contains(Path.GetExtension(f.Path))) continue;
                var playlistItem = new PlaylistItem(f.Path);
                this.Playlist.AddLast(playlistItem);
            }

            this._currentItem = this.Playlist.First;
            logger.Debug("Current item set to " + this._currentItem?.Value.Name);


            this.OnTrackChanged?.Invoke(this, new PlaylistEventArgs(this.CurrentItem));

        }


        public void Stop()
        {

            if (_currentItem == null) return;

            logger.Info("Stopping playback...");
            if (_waveOut != null)
            {
                logger.Debug("Disposing of wave out and mp3FileReader");
                this._waveOut.Dispose();
                this._waveOut = null;
                this._mp3FileReader.Dispose();
                this._mp3FileReader = null;
            }
            this.IsPlaying = false;
            this.IsPaused = false;
            logger.Debug("IsPlaying set to: " + this.IsPlaying);
        }

        public void Play()
        {
            if (_currentItem == null) return;

            if (this._waveOut == null)
            {
                this._initialisePlayer();
            }

            this._waveOut.Play();
            this.IsPlaying = true;
            this.IsPaused = false;

        }

        public void Pause()
        {
            if (_currentItem == null) return;

            _waveOut?.Pause();
            this.IsPaused = true;
        }

        public void NextTrack()
        {
            if( _currentItem == null) return;
            
            // We keep track if it was playing ready for later
            var wasPlaying = this.IsPlaying;
            var wasPaused = this.IsPaused;

            // We now stop if it is playing
            if (this.IsPlaying)
            {
                this.Stop();
            }

            // Move to the next track (or first if at the end)
            var next = _currentItem.Next;

            _currentItem = next ?? Playlist.First;

            this.OnTrackChanged?.Invoke(this, new PlaylistEventArgs(this.CurrentItem));

            // If it was playing originally then we play again
            if (!wasPlaying) return;

            this.Play();

            if (wasPaused)
            {
                this.Pause();
            }
        }

        public void PreviousTrack()
        {

            if(_currentItem == null) return;

            // We keep track if it was playing ready for later
            var wasPlaying = this.IsPlaying;
            var wasPaused = this.IsPaused;

            // We now stop if it is playing
            if (this.IsPlaying)
            {
                this.Stop();
            }

            // Move to the previous track (or last if at the end)
            var previous = _currentItem.Previous;

            _currentItem = previous ?? Playlist.Last;

            this.OnTrackChanged?.Invoke(this, new PlaylistEventArgs(this.CurrentItem));


            // If it was plahing originally then we play again
            if (!wasPlaying) return;

            this.Play();

            if (wasPaused)
            {
                this.Pause();
            }
        }

        private void _initialisePlayer()
        {
            logger.Debug("Initialising player with file: " + this.CurrentItem.Path);
            this._mp3FileReader = new MediaFoundationReader(this.CurrentItem.Path);
            this._waveOut = new WaveOut();
            this._waveOut.PlaybackStopped += this.OnPlaybackStopped;
            this._waveOut.Init(this._mp3FileReader);
        }

        private void OnPlaybackStopped(object obj, StoppedEventArgs args)
        {

            if (this.IsPlaying)
            {
                this.NextTrack();
            }
        }


        public void Dispose()
        {
            _mp3FileReader?.Dispose();
            _waveOut?.Dispose();
        }
    }
}