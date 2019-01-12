using TagLib;

namespace GrandadAudioPlayer.Model
{
    public class PlaylistItem
    {

        public string Name { get; }
        public string Path { get; }

        public string DisplayName => string.IsNullOrEmpty(Id3Tags?.Title) ? Name : Id3Tags.Title;

        public TagLib.Properties Id3Properties { get; }
        public Tag Id3Tags { get;  }

        public PlaylistItem(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);

            using (var f = File.Create(path))
            {
                Id3Properties = f.Properties;
                Id3Tags = f.Tag;
            }
        }
    }
}
