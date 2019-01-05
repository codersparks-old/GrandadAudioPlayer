using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandadAudioPlayer.Utils.Github
{
    public class GithubAsset
    {
        public string Url { get; set; }
        public string BrowserDownloadUrl { get; set; }
        public long Id { get; set; }
        public string NodeId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string State { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public long DownloadCount { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public GithubUser Uploader { get; set; }
    }
}
