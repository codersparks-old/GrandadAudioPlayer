using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GrandadAudioPlayer.Utils.Configuration;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Unity;

namespace GrandadAudioPlayer.Utils.Updater
{
    public class SchedulerConfiguration
    {

        private readonly ILog _log = LogManager.GetLogger(typeof(SchedulerConfiguration));

        private readonly ConfigurationManager _configurationManager;
        private readonly IUnityContainer _container;

        private SchedulerConfiguration(ConfigurationManager configurationManager, IUnityContainer container)
        {
            _configurationManager = configurationManager;
            _container = container;
        }
       

        public async void RunUpdateScheduler()
        {
            try
            {
                _log.Debug("Resolving IScheduler instance from container");
                var schedulerFactory = _container.Resolve<ISchedulerFactory>();

                var scheduler = await schedulerFactory.GetScheduler(CancellationToken.None);

                var cronString = _configurationManager.Configuration.UpdateCheckCron;
                _log.Info($"Scheduling job with cron {cronString}");

                await scheduler.ScheduleJob(
                    new JobDetailImpl("Updater Job", typeof(UpdaterJob)),
                    new CronTriggerImpl("Updater trigger", "Updater Group", cronString)
                );

                _log.Debug("Staring scheduler");
                await scheduler.Start();

                _log.Debug("Scheduler started");
            }
            catch (SchedulerException e)
            {
                _log.Error("Exception caught setting up update scheduler", e);
            }
        }

    }
}
