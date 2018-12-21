namespace GrandadAudioPlayer.Model.FolderView
{
    public abstract class FolderItemBase 
    {

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
