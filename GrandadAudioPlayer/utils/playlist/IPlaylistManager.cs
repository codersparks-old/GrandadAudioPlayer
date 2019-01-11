using System.Collections.ObjectModel;
using System.Windows.Input;
using GrandadAudioPlayer.Model;

namespace GrandadAudioPlayer.Utils.Playlist
{
    public interface IPlaylistManager
    {
        PlaylistItem CurrentItem { get; set; }
        string CurrentPosition { get; }
        double CurrentPositionPercentage { get; }
        bool IsPaused { get; }
        bool IsPlaying { get; }
        ObservableCollection<PlaylistItem> Playlist { get; }
        string RootFolder { get; set; }
        int Volume { get; set; }

        void NextTrack();
        void Pause();
        void Play();
        void PreviousTrack();
        void ReloadPlaylist();
        void Stop();
    }
}