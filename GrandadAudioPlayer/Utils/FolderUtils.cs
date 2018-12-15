using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandadAudioPlayer.Model.FolderView;

namespace GrandadAudioPlayer.Utils
{
    public class FolderUtils
    {

        public static List<FolderItemBase> GetTreeStructure(string root)
        {
            return _processDirectory(root);
        }

        public static List<FolderItemFile> GetFilesUnderFolder(string root)
        {
            var fileList = new List<FolderItemFile>();

            foreach (var folderItem in _processDirectory(root))
            {
                if (folderItem.GetType() == typeof(FolderItemFile))
                {
                    fileList.Add((FolderItemFile)folderItem);
                }
            }

            return fileList;
        }

        private static List<FolderItemBase> _processDirectory(string directory)
        {

            var directoryContents = new List<FolderItemBase>();

            if ((File.GetAttributes(directory) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                directoryContents.Add(new FolderItemFile(directory));
            }
            else
            {

                try
                {
                    foreach (var d in Directory.GetDirectories(directory))
                    {

                        var info = new DirectoryInfo(d);

                        if (info.Attributes.HasFlag(FileAttributes.Hidden))
                        {
                            continue;
                        }

                        var folderItemFolder = new FolderItemFolder(d);

                        var childContents = _processDirectory(d);

                        folderItemFolder.Children.AddRange(childContents);

                        directoryContents.Add(folderItemFolder);
                    }
                }
                catch (UnauthorizedAccessException e)
                {

                }

                try
                {
                    directoryContents.AddRange(
                        (from f in Directory.GetFiles(directory)
                         let info = new FileInfo(f)
                         where !info.Attributes.HasFlag(FileAttributes.Hidden)
                         select new FolderItemFile(f)).Cast<FolderItemBase>());
                }
                catch (UnauthorizedAccessException e)
                {

                }

            }

            return directoryContents;
        }
    }
}
