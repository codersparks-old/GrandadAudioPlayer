using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;

namespace GrandadAudioPlayer.Utils.Updater
{
    class UpdaterJob : IJob
    {
        private readonly Updater _updater;
        private readonly ILog _log = LogManager.GetLogger(typeof(UpdaterJob));

        public UpdaterJob(Updater updater)
        {
            _updater = updater;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _log.Info("Starting check for update job...");
            
            return Task.Run(() => _updater.Update());
        }
    }
}
