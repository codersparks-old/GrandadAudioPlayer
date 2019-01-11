using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandadAudioPlayer.Utils.Playlist;
using log4net;
using Prism.Commands;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class PlaylistControlsViewModel : BindableBase
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(PlaylistControlsViewModel));
        public PlaylistControlsViewModel(IPlaylistManager playlistManager)
        {
            PlaylistManager = playlistManager;

            PreviousCommand = new DelegateCommand(PreviousMethod, CanPreviousMethod)
                .ObservesProperty(() => PlaylistManager.CurrentItem);

            PlayCommand = new DelegateCommand(PlayMethod, CanPlayMethod)
                .ObservesProperty(() => PlaylistManager.IsPaused)
                .ObservesProperty(() => PlaylistManager.IsPlaying)
                .ObservesProperty(() => PlaylistManager.CurrentItem);

            PauseCommand = new DelegateCommand(PauseMethod, CanPauseMethod)
                .ObservesProperty(() => PlaylistManager.IsPaused)
                .ObservesProperty(() => PlaylistManager.IsPlaying)
                .ObservesProperty(() => PlaylistManager.CurrentItem);

            StopCommand = new DelegateCommand(StopMethod, CanStopMethod)
                .ObservesProperty(() => PlaylistManager.IsPlaying)
                .ObservesProperty(() => PlaylistManager.CurrentItem);

            NextCommand = new DelegateCommand(NextMethod, CanNextMethod)
                .ObservesProperty(() => PlaylistManager.CurrentItem);
        }

        public IPlaylistManager PlaylistManager { get; }

        public DelegateCommand PreviousCommand { get; }
        public DelegateCommand PlayCommand { get; }
        public DelegateCommand PauseCommand { get; }
        public DelegateCommand StopCommand { get; }
        public DelegateCommand NextCommand { get; }

        public void PlayMethod()
        {
            PlaylistManager.Play();
            //_startPositionTimer();
        }

        public bool CanPlayMethod()
        {
            var canPlay = PlaylistManager.CurrentItem != null && (!PlaylistManager.IsPlaying || PlaylistManager.IsPaused);
            Logger.Debug($"Current Item != null {PlaylistManager.CurrentItem != null}");
            Logger.Debug($"PlaylistManager.IsPause: {PlaylistManager.IsPaused}");
            Logger.Debug($"PlaylistManager.IsPlaying: {PlaylistManager.IsPlaying}");
            Logger.Debug($"Can Play {canPlay}");
            return canPlay;
        }

        public void PauseMethod()
        {
            PlaylistManager.Pause();
            //_stopPositionTimer();
        }

        public bool CanPauseMethod()
        {
            return PlaylistManager.CurrentItem != null && (PlaylistManager.IsPlaying && !PlaylistManager.IsPaused);
        }

        public void StopMethod()
        {
            PlaylistManager.Stop();
            //_stopPositionTimer();
        }

        public bool CanStopMethod()
        {
            return PlaylistManager.CurrentItem != null && PlaylistManager.IsPlaying;
        }

        public void NextMethod()
        {
            PlaylistManager.NextTrack();
        }

        public bool CanNextMethod()
        {
            return PlaylistManager.CurrentItem != null;
        }

        public void PreviousMethod()
        {
            PlaylistManager.PreviousTrack();
        }

        public bool CanPreviousMethod()
        {
            return PlaylistManager.CurrentItem != null;
        }

    }
}
