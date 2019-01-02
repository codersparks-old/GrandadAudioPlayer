using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using log4net;
using RestSharp;

namespace GrandadAudioPlayer.Utils.Github
{
    public class GithubFacade
    {

        private readonly ILog _log = LogManager.GetLogger(typeof(GithubFacade));

        private static readonly string Token = "c74f44cf42cf91d4621a6d12e5d1d99a0aaa6a05";

//        private static readonly  Lazy<GithubFacade> LazyInstance =
//            new Lazy<GithubFacade>(() => new GithubFacade());
//
//        public static GithubFacade Instance => LazyInstance.Value;

        public string LatestTag { get; private set;  }

        private readonly RestClient _restClient;

        private GithubFacade()
        {
            _restClient = new RestClient("https://api.github.com");
        }

        public string GetLatestTag()
        {
            var restRequest = _constructRequest(Method.GET, "/repos/codersparks/GrandadAudioPlayer/releases/latest");

            var response = _restClient.Execute<GithubRelease>(restRequest);
            

            if (response.ResponseStatus == ResponseStatus.Completed && response.IsSuccessful)
            {
                _log.Debug($"Get Latest Tag released: {response.Data.TagName}");
                LatestTag = response.Data.TagName;
                return LatestTag;
            }
            else
            {
                if (response.ResponseStatus != ResponseStatus.Completed)
                {
                    throw new GithubFailureException("Error when trying to connect to github", response.ErrorException);
                }
                else
                {
                    throw new GithubFailureException($"Status code: {response.StatusCode}, body: {response.Content}" );
                }
            }
        }

        public bool DownloadGrandadAudioPlayerZipIfNewer(string currentTag, out string downloadedFilePath)
        {
            var tag = GetLatestTag();
            _log.Debug($"Current Build Tag {currentTag}. Latest Build Tag {tag}");

            if (tag != currentTag)
            {
                _log.Info("Found different version on GitHub therefore downloading...");

                downloadedFilePath = DownloadGrandadAudioPlayerZip(tag);

                return true;
            }
            else
            {
                downloadedFilePath = null;
                return false;
            }
        }

        public string DownloadGrandadAudioPlayerZip()
        {
            var tag = GetLatestTag();
            return DownloadGrandadAudioPlayerZip(tag);
        }

        public string DownloadGrandadAudioPlayerZip(string tag)
        {
            var filename = Path.GetTempFileName();
            _log.Debug($"Attempting to write GrandadAudioPlayer to {filename}");

            using (var writer = File.OpenWrite(filename))
            {
                RestClient restClient = new RestClient("https://github.com");
                RestRequest request = new RestRequest($"/codersparks/GrandadAudioPlayer/releases/download/{tag}/GrandadAudioPlayer.zip");

                request.ResponseWriter = (responseStream) => responseStream.CopyTo(writer);
                var response = restClient.DownloadData(request);

                return filename;
            }
        }


        private RestRequest _constructRequest(Method method, string url, bool includeAccept=true)
        {
            var restRequest = new RestRequest(url) {Method = method};
            restRequest.AddHeader("Accept", @"application / vnd.github.v3 + json");
            restRequest.AddHeader("Authorization", $"token {Token}");

            return restRequest;



        }
    }
}
