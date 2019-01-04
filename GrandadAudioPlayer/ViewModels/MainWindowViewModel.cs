using GrandadAudioPlayer.Views;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace GrandadAudioPlayer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly AdminView _adminView;

        public static string PlaylistContentRegion = "Player.Content";
        public static string TitleContentRegion = "Player.Title";
        public static string PlayerControlsContentRegion = "Player.Controls";

        public DelegateCommand OpenAdminDialogCommand { get; }

        public MainWindowViewModel(IRegionManager regionManager, AdminView adminView)
        {
            _adminView = adminView;
            regionManager.RegisterViewWithRegion(TitleContentRegion, typeof(TitleView));
            regionManager.RegisterViewWithRegion(PlaylistContentRegion, typeof(PlaylistView));

            OpenAdminDialogCommand = new DelegateCommand(OpenAdminDialogMethod);

        }

        public async void OpenAdminDialogMethod()
        {
            await DialogHost.Show(_adminView, "AdminDialog");
        }

    }
}
