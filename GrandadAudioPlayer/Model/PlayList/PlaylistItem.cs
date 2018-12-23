using TagLib;

namespace GrandadAudioPlayer.Model.PlayList
{
    public class PlaylistItem
    {

        public string Name { get; }
        public string Path { get; }

        public string DisplayName
        {
            get
            {
                if (Id3Tags?.Title == null || Id3Tags.Title.Length < 1) return Name;

                return Id3Tags.Title;
            }
        }
        public TagLib.Properties Id3Properties { get; }
        public Tag Id3Tags { get; }

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