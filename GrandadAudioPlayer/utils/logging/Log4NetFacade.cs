using System;
using log4net;
using Prism.Logging;

namespace GrandadAudioPlayer.utils.logging
{
    public class Log4NetFacade : ILoggerFacade
    {
        private static readonly ILog Logger = LogManager.GetLogger("PrismLoggerFacade");

        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    Logger.Debug(message);
                    break;
                case Category.Exception:
                    Logger.Error(message);
                    break;
                case Category.Info:
                    Logger.Info(message);
                    break;
                case Category.Warn:
                    Logger.Warn(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), category, null);
            }
        }
    }
}
