using System.Threading;
using GrandadAudioPlayer.Utils.Configuration;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace GrandadAudioPlayer.Utils.Updater
{
    public class SchedulerConfiguration
    {
        private readonly ConfigurationManager _configurationManager;
        private readonly ISchedulerFactory _schedulerFactory;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(SchedulerConfiguration));

        public SchedulerConfiguration(ConfigurationManager configurationManager, ISchedulerFactory schedulerFactory)
        {
            _configurationManager = configurationManager;
            _schedulerFactory = schedulerFactory;
        }

        public async void RunUpdateScheduler()
        {
            try
            {

                var scheduler = await _schedulerFactory.GetScheduler(CancellationToken.None);

                Logger.Info("Scheduling updater job");
                await scheduler.ScheduleJob(
                    new JobDetailImpl("Update Job", typeof(UpdaterJob)), 
                    new CronTriggerImpl("Updater Trigger", "Updater Group", _configurationManager.Configuration.UpdateCheckCron));

                Logger.Debug("Starting updater scheduler");
                await scheduler.Start();
            }
            catch (SchedulerException e)
            {
                Logger.Error("Exception caught setting up update scheduler", e);
            }
        }
    }
}
