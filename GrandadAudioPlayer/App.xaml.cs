using System.Windows;
using log4net;
using Squirrel;

namespace GrandadAudioPlayer
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(App));

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _log.Debug("Initialising...");

            using (var updateManager = new UpdateManager(@"c:\SquirrelReleases\"))
            {
                await updateManager.UpdateApp();
            }
        }
    }
}
