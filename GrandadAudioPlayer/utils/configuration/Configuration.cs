using System;
using System.Collections.Generic;

namespace GrandadAudioPlayer.utils.configuration
{
    public class Configuration
    {
        public string FolderPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public string UpdateCheckCron { get; set; } = "0 0/30 * * * ?";

        public HashSet<string> AllowedExtensions { get; }

        public Configuration()
        {
            AllowedExtensions = new HashSet<string> { ".mp3", ".m4a" };
        }
    }
}
