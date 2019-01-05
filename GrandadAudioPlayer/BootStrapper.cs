using System.Windows;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Logging;
using GrandadAudioPlayer.Utils.Playlist;
using Microsoft.Practices.Unity;
using Prism.Unity;
using GrandadAudioPlayer.Views;
using log4net;
using Prism.Logging;

namespace GrandadAudioPlayer
{
    public class BootStrapper : UnityBootstrapper
    {
        private static readonly ILog Log4NetLogger = LogManager.GetLogger(typeof(BootStrapper));

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Log4NetLogger.Info("Displaying Main window");
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        
            Container.RegisterType<PlaylistManager>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ConfigurationManager>(new ContainerControlledLifetimeManager());
        }

        protected override ILoggerFacade CreateLogger()
        {
            
                Log4NetLogger.Debug("Creating Log4NetFacade");
                return new Log4NetFacade();
        }
    }
}
