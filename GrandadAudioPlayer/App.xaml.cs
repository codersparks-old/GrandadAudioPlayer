using System;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Github;
using GrandadAudioPlayer.Utils.Updater;
using GrandadAudioPlayer.ViewModel;
using log4net;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using SharpCompress.Archives.Zip;
using Squirrel;

namespace GrandadAudioPlayer
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private readonly ILog _log = LogManager.GetLogger(typeof(App));

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _log.Debug("Initialising...");

            if (System.Diagnostics.Debugger.IsAttached)
            {
                _log.Debug("Debugger is attached - Not using squirrel");
            }
            else
            {
                _log.Debug("No debugger attached - using squirrel config");
                _log.Debug("Checking for new version in new thread");
                var updateTask = Task.Run(() => { GrandadAudioPlayerUpdater.Instance.Update(); });
            }
        }
    }
}
