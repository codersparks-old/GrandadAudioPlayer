using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandadAudioPlayer.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class BuildTagAttribute : Attribute
    {
        public string BuildTag { get; }

        public BuildTagAttribute() : this(string.Empty) {}

        public BuildTagAttribute(string buildTag) {this.BuildTag = buildTag;}
    }
}
