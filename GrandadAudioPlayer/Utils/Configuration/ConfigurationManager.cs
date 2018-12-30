﻿using System;
using System.IO;
using log4net;
using Newtonsoft.Json;

namespace GrandadAudioPlayer.Utils.Configuration
{
    public sealed class ConfigurationManager
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigurationManager));

        private static readonly Lazy<ConfigurationManager> LazyInstance =
            new Lazy<ConfigurationManager>(() => new ConfigurationManager());

        public static ConfigurationManager Instance => LazyInstance.Value;

        public static string BaseConfigDirectory { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GrandadAudioPlayer");



        private ConfigurationModel _configurationModel;
        private readonly string _configFilePath = Path.Combine(BaseConfigDirectory, "config");
        private const string ConfigFile = "config.json";


        public ConfigurationModel Configuration
        {
            get {
                LoadConfiguration();
                return _configurationModel;
            }
        }

        private ConfigurationManager()
        {
            LoadConfiguration();
        }

        public void LoadConfiguration()
        {
            if (File.Exists(Path.Combine(_configFilePath, ConfigFile)))
            {
                _configurationModel =
                    JsonConvert.DeserializeObject<ConfigurationModel>(File.ReadAllText(Path.Combine(_configFilePath, ConfigFile)));
            }
            else
            {
                _configurationModel = new ConfigurationModel();
                SaveConfiguration(_configurationModel);
            }
        }

        public void SaveConfiguration(ConfigurationModel configuration)
        {

            _configurationModel = configuration;

            if (!Directory.Exists(_configFilePath))
            {
                Directory.CreateDirectory(_configFilePath);
            }
            File.WriteAllText(Path.Combine(_configFilePath, ConfigFile), JsonConvert.SerializeObject(_configurationModel,Formatting.Indented));
        }
    }
}