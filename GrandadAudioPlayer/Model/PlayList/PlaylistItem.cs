using TagLib;

namespace GrandadAudioPlayer.Model.PlayList
{
    public class PlaylistItem
    {

        public string Name { get; private set; }
        public string Path { get; private set; }
        public File Id3File { get; private set; }

        public PlaylistItem(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);

            this.Id3File = File.Create(this.Path);
        }
    }
}