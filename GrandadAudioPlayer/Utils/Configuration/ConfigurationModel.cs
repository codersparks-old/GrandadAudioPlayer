using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace GrandadAudioPlayer.Utils.Configuration
{
    public class ConfigurationModel : ObservableObject
    {
        public string FolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public HashSet<string> AllowedExtensions { get; private set; }

        public ConfigurationModel()
        {
            this.AllowedExtensions = new HashSet<string>();
            this.AllowedExtensions.Add(".mp3");
            this.AllowedExtensions.Add(".m4a");
        }
    }
}
