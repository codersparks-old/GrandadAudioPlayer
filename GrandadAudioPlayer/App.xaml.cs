using System.Windows;
using log4net;

namespace GrandadAudioPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Logger.Info(">>>>>>>>>>>>>>>>> Application Launched <<<<<<<<<<<<<<<<<<<<<");

            BootStrapper bs = new BootStrapper();
            bs.Run();
        }
    }
}
