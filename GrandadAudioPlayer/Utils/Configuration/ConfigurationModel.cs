using System;
using System.Collections.Generic;
using System.IO;
using GalaSoft.MvvmLight;

namespace GrandadAudioPlayer.Utils.Configuration
{
    public class ConfigurationModel : ObservableObject
    {

        public static string BaseConfigDirectory { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GrandadAudioPlayer");

        public string FolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public string SquirrelSourcesPath { get; set; }
        public string UpdateCheckCron { get; set; } = "0 0/30 * * * ?";
        public string LogLevel { get; set; } = "INFO";

        public HashSet<string> AllowedExtensions { get; }

        public ConfigurationModel()
        {
            AllowedExtensions = new HashSet<string> {".mp3", ".m4a"};
            SquirrelSourcesPath = Path.Combine(BaseConfigDirectory, @"Releases");
        }
    }
}
