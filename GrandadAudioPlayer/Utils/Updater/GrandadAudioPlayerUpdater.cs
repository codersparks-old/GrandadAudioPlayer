using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Github;
using GrandadAudioPlayer.ViewModel;
using log4net;
using Squirrel;

namespace GrandadAudioPlayer.Utils.Updater
{
    public class GrandadAudioPlayerUpdater
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(GrandadAudioPlayerUpdater));

        private readonly ConfigurationManager _configurationManager;
        private readonly GithubFacade _githubFacade;

        public GrandadAudioPlayerUpdater(ConfigurationManager configurationManager, GithubFacade githubFacade)
        {
            _configurationManager = configurationManager;
            _githubFacade = githubFacade;
        }

        private string _downloadedTag = null;

        public async void Update()
        {

            System.IO.Directory.CreateDirectory(_configurationManager.Configuration.SquirrelSourcesPath);

            try
            {

                var squirrelDir = _configurationManager.Configuration.SquirrelSourcesPath;

                _log.Debug("Attempting to download newer version if available");

                if (_githubFacade.DownloadGrandadAudioPlayerZipIfNewer(_downloadedTag ?? AdminViewModel.BuildTag, out var zipFilePath))
                {

                    _log.Debug($"Downloaded Version: {_githubFacade.LatestTag}");
                    _downloadedTag = _githubFacade.LatestTag;

                    _log.Debug($"GrandadAudioPlayer.zip downloaded to: {zipFilePath}");

                    _log.Debug("Attempting to unzip...");

                    _emptyDirectory(squirrelDir);

                    ZipFile.ExtractToDirectory(zipFilePath, squirrelDir);

                    using (var mgr = new UpdateManager(_configurationManager.Configuration.SquirrelSourcesPath))
                    {
                        var currentVersion = mgr.CurrentlyInstalledVersion();

                        _log.Info($"Currently installed version {currentVersion}");
                        await mgr.UpdateApp();
                    }
                }
                else
                {
                    _log.Debug("No newer version found therefore not downloading");
                }
            }
            catch (Exception exception)
            {
                _log.Error("Exception caught whilst trying to download latest version", exception);
            }
        }

        private void _emptyDirectory(string dirPath)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);
            _emptyDirectory(di);
        }

        private void _emptyDirectory(DirectoryInfo di)
        {

            foreach (var fi in di.GetFiles())
            {
                _log.Debug($"Found file {fi.Name}...deleting");
                fi.Delete();
            }

            foreach (var subFolder in di.GetDirectories())
            {
                _log.Debug($"Attempting to delete folder: {di.Name}");
                _emptyDirectory(subFolder);
                subFolder.Delete();
            }
        }
    }
}
