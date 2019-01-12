using GrandadAudioPlayer.Utils.Playlist;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class PlaylistViewModel : BindableBase
    {
        public IPlaylistManager PlaylistManager { get; }

        public PlaylistViewModel(IPlaylistManager playlistManager)
        {
            PlaylistManager = playlistManager;
        }

    }
}
