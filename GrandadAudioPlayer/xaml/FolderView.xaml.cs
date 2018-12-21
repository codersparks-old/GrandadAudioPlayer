using System.Windows;
using System.Windows.Controls;
using GrandadAudioPlayer.Model.FolderView;
using GrandadAudioPlayer.ViewModel;

namespace GrandadAudioPlayer.xaml
{
    /// <summary>
    /// Interaction logic for FolderView.xaml
    /// </summary>
    public partial class FolderView : UserControl
    {
        public FolderView()
        {
            InitializeComponent();
        }

        public FolderViewModel ViewModel => DataContext as FolderViewModel;

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (ViewModel == null) return;

            ViewModel.SelectedItem = e.NewValue as FolderItemBase;
        }
    }
}
