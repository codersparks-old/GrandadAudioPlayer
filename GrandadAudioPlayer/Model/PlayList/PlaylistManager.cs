using System;
using System.Collections.Generic;
using System.IO;
using GrandadAudioPlayer.Model.FolderView;
using GrandadAudioPlayer.Utils;
using GrandadAudioPlayerClassLibrary.Configuration;

namespace GrandadAudioPlayer.Model.PlayList
{
    public sealed class PlaylistManager
    {
        private static readonly List<string> AllowedExtensions = ConfigurationManager.Instance.Configuration.AllowedExtensions; 

        private static readonly  Lazy<PlaylistManager> _lazyInstance =
            new Lazy<PlaylistManager>(() => new PlaylistManager());

        public static PlaylistManager Instance => _lazyInstance.Value;

        private string _rootFolder;

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
        }

    }
}