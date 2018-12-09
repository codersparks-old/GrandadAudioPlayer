using Newtonsoft.Json;
using System;
using System.IO;

namespace GrandadAudioPlayerClassLibrary.Configuration
{
    public sealed class ConfigurationManager
    {
        private static readonly Lazy<ConfigurationManager> _lazyInstance =
            new Lazy<ConfigurationManager>(() => new ConfigurationManager());

        public static ConfigurationManager Instance => _lazyInstance.Value;

        private ConfigurationModel _configurationModel;
        private readonly string _configFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/gap";
        private readonly string _configFile = "config.json";


        public ConfigurationModel Configuration
        {
            get {
                this.LoadConfiguration();
                return _configurationModel;
            }
        }

        private ConfigurationManager()
        {
            this.LoadConfiguration();
        }

        public void LoadConfiguration()
        {
            if (File.Exists(Path.Combine(_configFilePath, _configFile)))
            {
                _configurationModel =
                    JsonConvert.DeserializeObject<ConfigurationModel>(File.ReadAllText(Path.Combine(_configFilePath, _configFile)));
            }
            else
            {
                _configurationModel = new ConfigurationModel();
                this.SaveConfiguration(_configurationModel);
            }
        }

        public void SaveConfiguration(ConfigurationModel configuration)
        {

            _configurationModel = configuration;

            if (!Directory.Exists(_configFilePath))
            {
                Directory.CreateDirectory(_configFilePath);
            }
            File.WriteAllText(Path.Combine(_configFilePath, _configFile), JsonConvert.SerializeObject(_configurationModel,Formatting.Indented));
        }
    }
}