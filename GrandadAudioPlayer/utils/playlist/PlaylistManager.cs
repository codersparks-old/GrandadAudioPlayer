using System;

namespace GrandadAudioPlayer.Utils.Playlist
{
    public class PlaylistManager
    {

        public int Identifier { get; }

        public PlaylistManager()
        {
            var random = new Random();
            Identifier = random.Next();
        }
    }
}
