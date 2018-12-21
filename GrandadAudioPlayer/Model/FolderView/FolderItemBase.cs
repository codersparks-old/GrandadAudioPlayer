using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GrandadAudioPlayer.Model.PlayList;

namespace GrandadAudioPlayer.Model.FolderView
{
    public abstract class FolderItemBase : ViewModelBase
    {
        public static FolderItemBase SelectedItem { get; set; } = null;

        public string Name { get; }
        public string Path { get; }

        protected FolderItemBase(string path)
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
