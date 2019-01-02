using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using GrandadAudioPlayer.Utils.Configuration;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace GrandadAudioPlayer.Utils.Logging
{
    public class GapLoggingManager
    {
        private readonly string _logDirectory;

        private readonly ConfigurationManager _configurationManager;

        public GapLoggingManager(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _logDirectory = Path.Combine(ConfigurationModel.BaseConfigDirectory, "logs");
        }


        public void InitialiseLogging(bool debug=false)
        {
            var hierarchy = (Hierarchy) LogManager.GetRepository();

            var pattern = new PatternLayout("%date[%thread] %-6level %logger - %message %exception %newline");
            pattern.ActivateOptions();

            var rollingFileAppender = new RollingFileAppender
            {
                AppendToFile = true,
                File = Path.Combine(_logDirectory, "GrandadAudioPlayer.Log"),
                Layout = pattern,
                MaxSizeRollBackups = 3,
                MaximumFileSize = "1MB",
                StaticLogFileName = true,
                RollingStyle = RollingFileAppender.RollingMode.Size
            };
            rollingFileAppender.ActivateOptions();

            hierarchy.Root.AddAppender(rollingFileAppender);

            if (debug)
            {
                var consoleAppender = new ConsoleAppender {Layout = pattern};
                hierarchy.Root.AddAppender(consoleAppender);
            }

            hierarchy.Root.Level = _parseLogLevel(_configurationManager.Configuration.LogLevel);
            hierarchy.Configured = true;


        }

        private static Level _parseLogLevel(string level)
        {
            switch (level.ToUpper())
            {
                case "OFF":
                    return Level.Off;
                case "CRITICAL":
                case "FATAL":
                    return Level.Fatal;
                case "ERROR":
                    return Level.Error;
                case "WARNING":
                case "WARN":
                    return Level.Warn;
                case "INFO":
                    return Level.Info;
                case "DEBUG":
                    return Level.Debug;
                default:
                    return Level.Info;

            } 
            {
                    
            }
        }
    }
}
