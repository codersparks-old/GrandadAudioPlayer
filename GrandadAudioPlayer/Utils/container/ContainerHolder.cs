using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandadAudioPlayer.Model.PlayList;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Github;
using GrandadAudioPlayer.Utils.Logging;
using GrandadAudioPlayer.Utils.Updater;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Strategies;

namespace GrandadAudioPlayer.Utils.container
{
    public class ContainerHolder
    {
        private static readonly Lazy<UnityContainer> LazyInstance =
            new Lazy<UnityContainer>(() =>
            {
                var container = new UnityContainer();
                container.AddNewExtension<Quartz.Unity.QuartzUnityExtension>();
                
                container.RegisterType<ConfigurationManager>(new SingletonLifetimeManager());
                container.RegisterType<PlaylistManager>(new SingletonLifetimeManager());
                container.RegisterType<GapLoggingManager>(new SingletonLifetimeManager());
                container.RegisterType<UpdaterJob>();
                container.RegisterType<GithubFacade>();
                container.RegisterType<SchedulerConfiguration>(new SingletonLifetimeManager());
                container.RegisterType<GrandadAudioPlayerUpdater>(new SingletonLifetimeManager());
                return container;
            });

        public static UnityContainer Container => LazyInstance.Value;
    }
}
