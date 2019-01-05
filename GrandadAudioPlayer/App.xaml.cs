using System.Windows;
using CommonServiceLocator;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Logging;
using GrandadAudioPlayer.Utils.Playlist;
using GrandadAudioPlayer.Views;
using log4net;
using log4net.Config;
using Prism.Ioc;
using Prism.Logging;

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
            containerRegistry.RegisterSingleton<PlaylistManager>();
            containerRegistry.RegisterSingleton<ConfigurationManager>();
            containerRegistry.RegisterSingleton<ILoggerFacade, Log4NetFacade>();
        }

        protected override Window CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();
        }
    }
}
