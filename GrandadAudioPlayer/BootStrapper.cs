using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Unity;
using GrandadAudioPlayer.Views;

namespace GrandadAudioPlayer
{
    public class BootStrapper : UnityBootstrapper
    {

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterTypeForNavigation<PlayerView>("PlayerView");
            Container.RegisterTypeForNavigation<AdminView>("AdminView");
        }
    }
}
