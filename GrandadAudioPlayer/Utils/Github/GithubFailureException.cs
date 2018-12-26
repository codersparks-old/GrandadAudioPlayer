using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandadAudioPlayer.Utils.Github
{
    public class GithubFailureException : Exception
    {
        public GithubFailureException()
        {
            
        }

        public GithubFailureException(string message) : base(message)
        {
        }

        public GithubFailureException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
