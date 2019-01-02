using System.Windows;
using GrandadAudioPlayer.Utils.container;
using GrandadAudioPlayer.Utils.Logging;
using GrandadAudioPlayer.Utils.Updater;
using log4net;
using Quartz;
using Unity;

namespace GrandadAudioPlayer
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private readonly ILog _log = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GapLoggingManager loggingManager = ContainerHolder.Container.Resolve<GapLoggingManager>();
            loggingManager.InitialiseLogging(System.Diagnostics.Debugger.IsAttached);

            _log.Debug("Initialising...");

            if (System.Diagnostics.Debugger.IsAttached)
            {
                _log.Debug("Debugger is attached - Not using squirrel");
            }
            else
            {
                _log.Debug("No debugger attached - using squirrel config");
                _log.Debug("Checking for new version in new thread");
                ContainerHolder.Container.Resolve<SchedulerConfiguration>().RunUpdateScheduler();
            }
        }

        
    }
}
