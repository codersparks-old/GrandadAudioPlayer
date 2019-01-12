using System.Collections.Generic;

namespace GrandadAudioPlayer.Utils.Github
{
    public class GithubRelease
    {
        public string Url { get; set; }
        public string HtmlUrl { get; set; }
        public string AssetsUrl { get; set; }
        public string UploadUrl { get; set; }
        public string TarballUrl { get; set; }
        public string ZipballUrl { get; set; }
        public long Id { get; set; }
        public string NodeId { get; set; }
        public string TagName { get; set; }
        public string TargetCommitish { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public bool Draft { get; set; }
        public bool Prerelease { get; set; }
        public string CreatedAt { get; set; }
        public string PublishedAt { get; set; }
        public GithubUser Author { get; set; }
        public List<GithubAsset> Assets { get; set; }
    }
}
