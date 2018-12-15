using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace GrandadAudioPlayerClassLibrary.Configuration
{
    public class ConfigurationModel : ObservableObject
    {
        public string FolderPath { get; set; } = @"C:\music\";
        public List<string> AllowedExtensions { get; set; } = new List<string>(new string[] {".mp3"});
    }
}
