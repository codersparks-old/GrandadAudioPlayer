using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandadAudioPlayer.Model.PlayList;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Logging;
using Unity;

namespace GrandadAudioPlayer.Utils.container
{
    public class ContainerHolder
    {
        private static readonly Lazy<UnityContainer> LazyInstance =
            new Lazy<UnityContainer>(() =>
            {
                var container = new UnityContainer();

                container.RegisterSingleton<ConfigurationManager>();
                container.RegisterSingleton<PlaylistManager>();
                container.RegisterSingleton<GapLoggingManager>();
                return container;
            });

        public static UnityContainer Container => LazyInstance.Value;
    }
}
