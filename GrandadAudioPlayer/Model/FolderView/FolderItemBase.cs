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

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
                if (!_isSelected) return;

                SelectedItem = this;

                PlaylistMessage messageBody = new PlaylistMessage(this.Path);
                Messenger.Default.Send<NotificationMessage<PlaylistMessage>>(new NotificationMessage<PlaylistMessage>(messageBody, "Playlist Updated"));
            }
        }

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
