using GalaSoft.MvvmLight;
using TagLib;

namespace GrandadAudioPlayer.Model.PlayList
{
    public class PlaylistItem : ViewModelBase
    {

        public static PlaylistItem SelectedItem { get; set; } = null;

        public string Name { get; private set; }
        public string Path { get; private set; }
        public TagLib.Properties Id3Properties { get; private set; }

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
            }
        }

        public PlaylistItem(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);

            using (TagLib.File f = TagLib.File.Create(path))
            {
                this.Id3Properties = f.Properties;
            }
        }
    }
}