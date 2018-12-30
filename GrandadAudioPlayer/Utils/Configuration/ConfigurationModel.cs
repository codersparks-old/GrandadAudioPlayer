using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace GrandadAudioPlayer.Utils.Configuration
{
    public class ConfigurationModel : ObservableObject
    {
        public string FolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public string SquirrelSourcesPath { get; set; } = @"C:\GrandadAudioPlayer\Releases";
        public string UpdateCheckCron { get; set; } = "0 0/5 * * * ?";

        public HashSet<string> AllowedExtensions { get; }

        public ConfigurationModel()
        {
            AllowedExtensions = new HashSet<string> {".mp3", ".m4a"};
        }
    }
}
