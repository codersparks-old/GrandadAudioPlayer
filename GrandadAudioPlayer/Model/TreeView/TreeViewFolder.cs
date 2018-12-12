using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandadAudioPlayer.Model.TreeView
{
    public class TreeViewFolder : TreeViewBase
    {
        public List<TreeViewBase> Children { get; } = new List<TreeViewBase>();

        public TreeViewFolder(string name) : base(name)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(base.ToString());

            builder.Append("Children=[");
            foreach (var item in this.Children)
            {
                builder.Append(item.ToString());
                builder.Append(',');
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}
