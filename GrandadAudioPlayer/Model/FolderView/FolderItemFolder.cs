using System.Collections.Generic;
using System.Text;

namespace GrandadAudioPlayer.Model.FolderView
{
    public class FolderItemFolder : FolderItemBase
    {
        public List<FolderItemBase> Children { get; } = new List<FolderItemBase>();

        public FolderItemFolder(string name) : base(name)
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
