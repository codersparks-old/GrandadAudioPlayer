using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GrandadAudioPlayer.Utils.Configuration;
using log4net;
using Quartz;
using Quartz.Impl;

namespace GrandadAudioPlayer.Utils.Updater
{
    public class SchedulerConfiguration
    {
//        private static readonly Lazy<SchedulerConfiguration> LazyInstance =
//            new Lazy<SchedulerConfiguration>(() => new SchedulerConfiguration());
//
//        public static SchedulerConfiguration Instance => LazyInstance.Value;

        private readonly ILog _log = LogManager.GetLogger(typeof(SchedulerConfiguration));

        private readonly ConfigurationManager _configurationManager;

        private SchedulerConfiguration(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public IScheduler Scheduler { get; private set; } = null;

        public async void RunUpdateScheduler()
        {
            try
            {

                if (Scheduler == null) await _initialize();

                await Scheduler.Start();

                var updateJob = JobBuilder.Create<UpdaterJob>()
                    .WithIdentity("Updater Job", "Updater Group")
                    .Build();

                var cronString = _configurationManager.Configuration.UpdateCheckCron;
                _log.Debug($"Cron loaded from config {cronString}");
                var updateTrigger = TriggerBuilder.Create()
                    .WithIdentity("Updater Trigger", "Updater Group")
                    .StartNow()
                    .WithCronSchedule(cronString)
                    .ForJob(updateJob)
                    .Build();

                await Scheduler.ScheduleJob(updateJob, updateTrigger);

                _log.Debug("Scheduler started");
            }
            catch (SchedulerException e)
            {
                _log.Error("Exception caught setting up update scheduler", e);
            }
        }

        public async Task _initialize()
        {
            NameValueCollection props = new NameValueCollection
            {
                {"quartz.serializer.type", "binary"}
            };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            Scheduler = await factory.GetScheduler();
        }
    }
}
