using System.Diagnostics;
using log4net;
using Prism.Commands;
using Prism.Mvvm;

namespace GrandadAudioPlayer.ViewModels
{
    public class TitleViewModel : BindableBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TitleViewModel));

        public TitleViewModel()
        {
            PowerCommand = new DelegateCommand(PowerMethod);
        }

        public DelegateCommand PowerCommand { get; }

        public void PowerMethod()
        {
            if (Debugger.IsAttached)
            {
                Logger.Warn("Detected debugger attached therefore disabling shutdown functionality");
            }
            else
            {
                Logger.Info("User pressed 'Turn Off' button, shutting down computer");
                Process.Start("shutdown.exe", "-s -t 00");
            }

        }
    }
}
