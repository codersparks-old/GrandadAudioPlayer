using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandadAudioPlayer.Model.TreeView
{
    public abstract class TreeViewBase
    {

        public string Name { get; }
        public string Path { get; }

        protected TreeViewBase(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);

        }

        public override string ToString()
        {
            return this.GetType().Name + ": " + Name;
        }
    }
}
