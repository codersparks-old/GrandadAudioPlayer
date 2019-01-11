using System.Windows;
using CommonServiceLocator;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Logging;
using GrandadAudioPlayer.Utils.Playlist;
using GrandadAudioPlayer.Utils.Updater;
using GrandadAudioPlayer.Views;
using log4net;
using log4net.Config;
using Prism.Ioc;
using Prism.Logging;
using Prism.Unity;
using Quartz.Unity;

namespace GrandadAudioPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(App));

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ConfigurationManager>();
            containerRegistry.RegisterSingleton<ILoggerFacade, Log4NetFacade>();
            containerRegistry.RegisterSingleton<SchedulerConfiguration>();
            containerRegistry.RegisterSingleton<IPlaylistManager, PlaylistManager>();
            containerRegistry.Register<Updater>();
            containerRegistry.GetContainer().AddExtension(new QuartzUnityExtension());
        }

        protected override Window CreateShell()
        {

            var configManager = ServiceLocator.Current.GetInstance<ConfigurationManager>();

            Logger.Info($">>>>>>>>>> Starting Grandad Audio Player. Version: {configManager.Version} Build: {configManager.BuildTag} <<<<<<<<<<");
            var schedulerConfiguration = ServiceLocator.Current.GetInstance<SchedulerConfiguration>();
            schedulerConfiguration.RunUpdateScheduler();
            return ServiceLocator.Current.GetInstance<MainWindow>();
        }
    }
}
