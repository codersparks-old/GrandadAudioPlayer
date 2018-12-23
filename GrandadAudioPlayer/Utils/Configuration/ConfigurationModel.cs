using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace GrandadAudioPlayer.Utils.Configuration
{
    public class ConfigurationModel : ObservableObject
    {
        public string FolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public HashSet<string> AllowedExtensions { get; }

        public ConfigurationModel()
        {
            AllowedExtensions = new HashSet<string> {".mp3", ".m4a"};
        }
    }
}
