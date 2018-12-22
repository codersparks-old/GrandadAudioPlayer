using TagLib;

namespace GrandadAudioPlayer.Model.PlayList
{
    public class PlaylistItem
    {

        public string Name { get; private set; }
        public string Path { get; private set; }

        public string DisplayName
        {
            get
            {
                if (Id3Tags?.Title == null || Id3Tags.Title.Length < 1) return this.Name;

                return Id3Tags.Title;
            }
        }
        public TagLib.Properties Id3Properties { get; private set; }
        public Tag Id3Tags { get; private set; }

        public PlaylistItem(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);

            using (var f = File.Create(path))
            {
                this.Id3Properties = f.Properties;
                this.Id3Tags = f.Tag;
            }
        }
    }
}