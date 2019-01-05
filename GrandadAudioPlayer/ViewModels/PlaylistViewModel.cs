using GrandadAudioPlayer.Utils.Playlist;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class PlaylistViewModel : BindableBase
    {

        private readonly PlaylistManager _playlistManager;

        public int PlaylistManagerIdentifier => _playlistManager.Identifier;

        public PlaylistViewModel(PlaylistManager playlistManager)
        {
            _playlistManager = playlistManager;
        }

    }
}
