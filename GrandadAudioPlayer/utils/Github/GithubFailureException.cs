using System;

namespace GrandadAudioPlayer.Utils.Github
{
    public class GithubFailureException : Exception
    {
        public GithubFailureException(string message) : base(message)
        {
        }

        public GithubFailureException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
