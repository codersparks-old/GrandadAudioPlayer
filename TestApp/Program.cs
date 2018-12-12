using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandadAudioPlayer.Utils;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = FolderTreeBuilder.getTreeStructure(@"C:\Users\Coder Sparks\Documents\GapMusic");

            foreach (var i in list)
            {
                Console.WriteLine(i.ToString());
            }
            Console.ReadKey();
        }
    }
}
