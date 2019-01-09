using GrandadAudioPlayer.Utils.Playlist;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class PlaylistViewModel : BindableBase
    {
        public PlaylistManager PlaylistManager { get; }

        public PlaylistViewModel(PlaylistManager playlistManager)
        {
            PlaylistManager = playlistManager;
        }

    }
}
