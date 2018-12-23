namespace GrandadAudioPlayer.Model.PlayList
{
    public class PlaylistMessage
    {

        public string Path { get; }

        public PlaylistMessage(string path)
        {
            Path = path;
        }
        
    }
}