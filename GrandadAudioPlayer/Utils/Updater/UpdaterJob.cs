using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Quartz.Logging;

namespace GrandadAudioPlayer.Utils.Updater
{
    public class UpdaterJob : IJob
    {

        private readonly ILog _log = LogManager.GetLogger(typeof(UpdaterJob));

        public Task Execute(IJobExecutionContext context)
        {
            _log.Info("Starting check for update job...");

            return Task.Run(() => GrandadAudioPlayerUpdater.Instance.Update());
        }
    }
}
