using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandadAudioPlayer.Model.TreeView;

namespace GrandadAudioPlayer.Utils
{
    public class FolderTreeBuilder
    {

        public static List<TreeViewBase> getTreeStructure(string root)
        {
            return _processDirectory(root);
        }

        private static List<TreeViewBase> _processDirectory(string directory)
        {

            List<TreeViewBase> directoryContents = new List<TreeViewBase>();

            try
            {
                foreach (string d in Directory.GetDirectories(directory))
                {

                    DirectoryInfo info = new DirectoryInfo(d);

                    if (info.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        continue;
                    }

                    TreeViewFolder folder = new TreeViewFolder(d);

                    List<TreeViewBase> childContents = _processDirectory(d);

                    folder.Children.AddRange(childContents);

                    directoryContents.Add(folder);
                }
            }
            catch (UnauthorizedAccessException e)
            {

            }

            try
            {
                foreach (string f in Directory.GetFiles(directory))
                {
                    FileInfo info = new FileInfo(f);

                    if (info.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        continue;
                    }
                    directoryContents.Add(new TreeViewFile(f));
                }
            }
            catch (UnauthorizedAccessException e)
            {

            }

            return directoryContents;
        }
    }
}
