/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:GrandadAudioPlayer"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace GrandadAudioPlayer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<FolderViewModel>();
            SimpleIoc.Default.Register<PlaylistViewModel>();
            SimpleIoc.Default.Register<DialogsViewModel>();
            SimpleIoc.Default.Register<AdminViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public FolderViewModel FolderViewModel => ServiceLocator.Current.GetInstance<FolderViewModel>();

        public PlaylistViewModel PlaylistViewModel => ServiceLocator.Current.GetInstance<PlaylistViewModel>();

        public DialogsViewModel DialogsViewModel => ServiceLocator.Current.GetInstance<DialogsViewModel>();

        public AdminViewModel AdminViewModel => ServiceLocator.Current.GetInstance<AdminViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}