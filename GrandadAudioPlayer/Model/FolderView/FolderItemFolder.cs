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
            var builder = new StringBuilder(base.ToString());

            builder.Append("Children=[");
            foreach (var item in Children)
            {
                builder.Append(item);
                builder.Append(',');
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}
