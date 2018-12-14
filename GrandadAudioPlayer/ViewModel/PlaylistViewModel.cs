

using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GrandadAudioPlayer.Model.PlayList;

namespace GrandadAudioPlayer.ViewModel
{
    public class PlaylistViewModel : ViewModelBase
    {
        public PlaylistViewModel()
        {
            Messenger.Default.Register<NotificationMessage<PlaylistMessage>>(this, PlaylistUpdate );
        }

        public void PlaylistUpdate(NotificationMessage<PlaylistMessage> message)
        {
            MessageBox.Show("Message received " + message.Content.Path);
        }
    }

    
}