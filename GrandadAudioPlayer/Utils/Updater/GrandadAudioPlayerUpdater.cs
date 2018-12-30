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

        private static readonly Lazy<GrandadAudioPlayerUpdater> LazyInstance =
            new Lazy<GrandadAudioPlayerUpdater>(() => new GrandadAudioPlayerUpdater());

        public static GrandadAudioPlayerUpdater Instance => LazyInstance.Value;

        private string _dowloadedTag = null;

        public async void Update()
        {

            System.IO.Directory.CreateDirectory(ConfigurationManager.Instance.Configuration.SquirrelSourcesPath);

            try
            {

                var squirrelDir = ConfigurationManager.Instance.Configuration.SquirrelSourcesPath;

                var github = GithubFacade.Instance;

                _log.Debug("Attempting to download newer version if available");

                if (github.DownloadGrandadAudioPlayerZipIfNewer(_dowloadedTag ?? AdminViewModel.BuildTag, out var zipFilePath))
                {

                    _log.Debug($"Downloaded Version: {github.LatestTag}");
                    _dowloadedTag = github.LatestTag;

                    _log.Debug($"GrandadAudioPlayer.zip downloaded to: {zipFilePath}");

                    _log.Debug("Attempting to unzip...");

                    _emptyDirectory(squirrelDir);

                    ZipFile.ExtractToDirectory(zipFilePath, squirrelDir);

                    using (var mgr = new UpdateManager(ConfigurationManager.Instance.Configuration.SquirrelSourcesPath))
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
