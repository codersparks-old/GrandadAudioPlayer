using System.Collections.Generic;
using System.IO;
using System.Linq;
using GrandadAudioPlayer.Model;

namespace GrandadAudioPlayer.Utils.Playlist
{
    public class FileUtils
    { 

        public static IEnumerable<string> GetAllFilesWithAllowedExtensionUnderDirectory(string path, HashSet<string> allowedExtensions, bool recurse=true)
        {

            var searchOption = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            return Directory.EnumerateFiles(path, "*", searchOption)
                .Where(file => ! new FileInfo(file).Attributes.HasFlag(FileAttributes.Hidden))
                .Where(file => allowedExtensions.Contains(Path.GetExtension(file)));
        }

        public IEnumerable<PlaylistItem> GetPlaylistItemsUnderDirectory(string path, HashSet<string> allowedExtensions, bool recurse=true)
        {

            return GetAllFilesWithAllowedExtensionUnderDirectory(path, allowedExtensions, recurse).Select(FileToPlaylistItem);
        }

        private static PlaylistItem FileToPlaylistItem(string file)
        {
            return new PlaylistItem(file);
        }
    }
}
