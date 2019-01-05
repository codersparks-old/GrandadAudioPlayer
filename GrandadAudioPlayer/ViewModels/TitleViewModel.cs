using GrandadAudioPlayer.Utils.Playlist;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class TitleViewModel : BindableBase
    {

        private readonly PlaylistManager _playlistManager;

        public int PlaylistManagerIdentifier => _playlistManager.Identifier;

        public TitleViewModel(PlaylistManager playlistManager)
        {
            _playlistManager = playlistManager;
        }
    }
}
