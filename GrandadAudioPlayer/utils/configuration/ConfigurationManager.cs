using System;
using System.IO;
using Newtonsoft.Json;

namespace GrandadAudioPlayer.utils.configuration
{
    public class ConfigurationManager
    {
        private const string ConfigFile = "config.json";
        private const string ConfigurationDirectoryName = "configuration";
        private const string ReleasesDirectoryName = "releases";
        private const string LogDirectoryName = "logs";

        public string AppDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GrandadAudioPlayer");

        public string ConfigDirectory => Path.Combine(AppDirectory, ConfigurationDirectoryName);
        public string ReleasesDirectory => Path.Combine(AppDirectory, ReleasesDirectoryName);
        public string LogDirectory => Path.Combine(AppDirectory, LogDirectoryName);

        public Configuration Configuration { get; private set;  }


        public ConfigurationManager()
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            if (!Directory.Exists(ReleasesDirectory))
            {
                Directory.CreateDirectory(ReleasesDirectory);
            }

            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            LoadConfiguration();
        }

        public void LoadConfiguration()
        {
            var configFile = Path.Combine(ConfigDirectory, ConfigFile);
            if (File.Exists(configFile))
            {
                Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configFile));
            }
            else
            {
                Configuration = new Configuration();
                SaveConfiguration(Configuration);
            }
        }

        public void SaveConfiguration(Configuration configuration)
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }
            File.WriteAllText(Path.Combine(ConfigDirectory, ConfigFile), JsonConvert.SerializeObject(configuration, Formatting.Indented));

        }
    }
}
