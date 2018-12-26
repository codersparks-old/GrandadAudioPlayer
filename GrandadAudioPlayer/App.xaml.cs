using System;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.ViewModel;
using log4net;
using Squirrel;

namespace GrandadAudioPlayer
{
    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private static readonly string Token = "c74f44cf42cf91d4621a6d12e5d1d99a0aaa6a05";
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

                System.IO.Directory.CreateDirectory(ConfigurationManager.Instance.Configuration.SquirrelSourcesPath);
               

                using (var mgr = new UpdateManager(ConfigurationManager.Instance.Configuration.SquirrelSourcesPath))
                {

                    var currentVersion = mgr.CurrentlyInstalledVersion();

                    _log.Info($"Currently installed version {currentVersion}");
                    await mgr.UpdateApp();
                }
            }
        }
    }
}
