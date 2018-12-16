using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GrandadAudioPlayer.Model.FolderView;
using GrandadAudioPlayer.Utils;
using GrandadAudioPlayerClassLibrary.Configuration;
using NAudio.Wave;

namespace GrandadAudioPlayer.Model.PlayList
{
    public sealed class PlaylistManager : IDisposable
    {
        private static readonly List<string> AllowedExtensions = ConfigurationManager.Instance.Configuration.AllowedExtensions; 

        private static readonly  Lazy<PlaylistManager> _lazyInstance =
            new Lazy<PlaylistManager>(() => new PlaylistManager());

        public static PlaylistManager Instance => _lazyInstance.Value;

        private string _rootFolder;
        private bool _isPlaying = false;
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

        public PlaylistItem CurrentItem => this._currentItem.Value;

        public LinkedList<PlaylistItem> Playlist { get; } = new LinkedList<PlaylistItem>();


        public void ReloadPlaylist()
        {
            this.Playlist.Clear();

            var files = FolderUtils.GetFilesUnderFolder(this._rootFolder);

            foreach (var f in files)
            {
                if (!AllowedExtensions.Contains(Path.GetExtension(f.Path))) continue;
                var playlistItem = new PlaylistItem(f.Path);
                this.Playlist.AddLast(playlistItem);
            }

            this._currentItem = this.Playlist.First;

        }

        public void Stop()
        {
            if (_waveOut != null)
            {
                this._waveOut.Dispose();
                this._waveOut = null;
                this._mp3FileReader.Dispose();
                this._mp3FileReader = null;
            }
            this._isPlaying = false;
        }

        public void PlayPause()
        {

            if (! _isPlaying)
            {
                if (this._waveOut == null)
                {
                    this._initialisePlayer();
                }

                this._waveOut.Play();
                this._isPlaying = true;
            }
            else
            {
                if (this._waveOut != null)
                {
                    this._waveOut.Pause();
                }
            }
        }

        public void NextTrack()
        {
            // We keep track if it was playing ready for later
            var wasPlaying = this._isPlaying;

            // We now stop if it is playing
            if (this._isPlaying)
            {
                this.Stop();
            }

            // Move to the next track (or first if at the end)
            var next = _currentItem.Next;

            if (next == null)
            {
                // We have to get first and then call next as it points to before 
                // the start of the collection
                _currentItem = Playlist.First;
            }
            else
            {
                _currentItem = next;
            }

            // If it was playing originally then we play again
            if (wasPlaying)
            {
                this.PlayPause();
            }
        }

        public void PreviousTrack()
        {

            // We keep track if it was playing ready for later
            var wasPlaying = this._isPlaying;

            // We now stop if it is playing
            if (this._isPlaying)
            {
                this.Stop();
            }

            // Move to the previous track (or last if at the end)
            var previous = _currentItem.Previous;

            if (previous == null)
            {
                
                _currentItem = Playlist.Last;
            }
            else
            {
                _currentItem = previous;
            }

            // If it was plahing originally then we play again
            if (wasPlaying)
            {
                this.PlayPause();
            }
        }

        private void _initialisePlayer()
        {
            this._mp3FileReader = new MediaFoundationReader(this.CurrentItem.Path);
            this._waveOut = new WaveOut();
            this._waveOut.PlaybackStopped += this.OnPlaybackStopped;
            this._waveOut.Init(this._mp3FileReader);
        }

        private void OnPlaybackStopped(object obj, StoppedEventArgs args)
        {
            
            if (this._isPlaying)
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