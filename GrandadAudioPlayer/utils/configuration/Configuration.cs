using System;
using System.Collections.Generic;

namespace GrandadAudioPlayer.Utils.Configuration
{
    public class Configuration
    {
        public string FolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public string UpdateCheckCron { get; set; } = "0 0/30 * * * ?";

        public bool AutoRestartOnUpdate { get; set; } = false;

        public HashSet<string> AllowedExtensions { get; }

        public Configuration()
        {
            AllowedExtensions = new HashSet<string> { ".mp3", ".m4a" };
        }
    }
}
