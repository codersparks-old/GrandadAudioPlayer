using System;
using System.IO;
using System.Reflection;
using GrandadAudioPlayer.Attributes;
using Newtonsoft.Json;

namespace GrandadAudioPlayer.Utils.Configuration
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

        public string DataDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GrandadAudioPlayerData");

        public string ConfigDirectory => Path.Combine(DataDirectory, ConfigurationDirectoryName);
        public string LogDirectory => Path.Combine(DataDirectory, LogDirectoryName);
        public string ReleasesDirectory => Path.Combine(AppDirectory, ReleasesDirectoryName);

        public Configuration Configuration { get; private set;  }

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string BuildTag
        {
            get
            {
                string buildTag = null;
                Type propertyType = null;

                var assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    propertyType = assembly.EntryPoint.ReflectedType;
                }

                if (propertyType == null) return null;

                var objects =
                    propertyType.Module.Assembly.GetCustomAttributes(typeof(BuildTagAttribute), false);

                if (objects.Length > 0)
                {
                    buildTag = ((BuildTagAttribute)objects[0]).BuildTag;
                }

                return buildTag;
            }
        }


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
                SaveConfiguration();
            }
        }

        public void SaveConfiguration()
        {
            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }
            File.WriteAllText(Path.Combine(ConfigDirectory, ConfigFile), JsonConvert.SerializeObject(Configuration, Formatting.Indented));

        }
    }
}
