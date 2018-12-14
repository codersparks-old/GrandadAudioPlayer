using System;

namespace GrandadAudioPlayer.Model.PlayList
{
    public class PlaylistMessage
    {

        public string Path { get; private set; }

        public PlaylistMessage(string path)
        {
            this.Path = path;
        }
        
    }
}