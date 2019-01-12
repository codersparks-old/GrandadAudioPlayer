using System;
using System.IO;
using System.IO.Compression;
using GrandadAudioPlayer.Utils.Configuration;
using GrandadAudioPlayer.Utils.Github;
using log4net;
using Squirrel;

namespace GrandadAudioPlayer.Utils.Updater
{
    public class Updater
    {
        private readonly ConfigurationManager _configurationManager;
        private readonly ILog _log = LogManager.GetLogger(typeof(Updater));

        private string _downloadedTag;

        public Updater(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public async void Update()
        {

            Directory.CreateDirectory(_configurationManager.ReleasesDirectory);

            try
            {

                var squirrelDir = _configurationManager.ReleasesDirectory;

                var github = GithubFacade.Instance;

                _log.Debug("Attempting to download newer version if available");

                if (github.DownloadGrandadAudioPlayerZipIfNewer(_downloadedTag ?? _configurationManager.BuildTag, out var zipFilePath))
                {
                    _log.Info("Newer version found in GitHub releases...downloading");
                    _log.Debug($"Downloaded Version: {github.LatestTag}");
                    _downloadedTag = github.LatestTag;

                    _log.Debug($"GrandadAudioPlayer.zip downloaded to: {zipFilePath}");

                    _log.Debug("Attempting to unzip...");

                    _emptyDirectory(squirrelDir);

                    ZipFile.ExtractToDirectory(zipFilePath, squirrelDir);

                    using (var mgr = new UpdateManager(_configurationManager.ReleasesDirectory))
                    {
                        var currentVersion = mgr.CurrentlyInstalledVersion();

                        _log.Info($"Currently installed version {currentVersion}");
                        await mgr.UpdateApp();

                        if (_configurationManager.Configuration.AutoRestartOnUpdate)
                        {
                            UpdateManager.RestartApp();
                        }
                    }
                }
                else
                {
                    _log.Info("No newer version found therefore not downloading");
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
