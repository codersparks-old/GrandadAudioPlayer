using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GrandadAudioPlayer.Model.FolderView;
using GrandadAudioPlayer.xaml;
using MaterialDesignThemes.Wpf;

namespace GrandadAudioPlayer.ViewModel
{
    /// <inheritdoc />
    public class DialogsViewModel : ViewModelBase
    {
        public DialogsViewModel()
        {
            OpenAdminDialogCommand = new RelayCommand(OpenAdminDialogMethod);
        }

        public ICommand OpenAdminDialogCommand { get; set; }

        public async void OpenAdminDialogMethod()
        {
            var view = new AdminDialog();

            await DialogHost.Show(view, "RootDialog");
            Messenger.Default.Send(new NotificationMessage<FolderMessage>(new FolderMessage(), "Root Folder Updated"));
        }
    }
}
