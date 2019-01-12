using System;

namespace GrandadAudioPlayer.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildTagAttribute : Attribute
    {
        public string BuildTag { get; }

        public BuildTagAttribute() : this(string.Empty) { }

        public BuildTagAttribute(string buildTag) { BuildTag = buildTag; }
    }
    
}
