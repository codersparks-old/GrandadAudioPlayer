namespace GrandadAudioPlayer.Model.FolderView
{
    public abstract class FolderItemBase 
    {

        public string Name { get; }
        public string Path { get; }

        protected FolderItemBase(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);

        }

        public override string ToString()
        {
            return GetType().Name + ": " + Name;
        }
    }
}
