using System;

namespace GrandadAudioPlayer.utils.playlist
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
