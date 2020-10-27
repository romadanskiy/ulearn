using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskTree
{
    public class Folder
    {
        public string FolderName;
        public Dictionary<string, Folder> Subfolders = new Dictionary<string, Folder>();

        public Folder(string name)
        {
            FolderName = name;
        }

        public Folder FindFolder(string subFolder)
        {
            if (Subfolders.TryGetValue(subFolder, out Folder folder))
                return folder;

            Subfolders[subFolder] = new Folder(subFolder);
            return Subfolders[subFolder];
        }

        public List<string> GetResult(int spacesCount, List<string> list)
        {
            if (FolderName != "some")
            {
                list.Add(new string(' ', spacesCount) + FolderName);
                spacesCount++;
            }

            foreach (var subfolder in Subfolders.Values.OrderBy(i => i.FolderName, StringComparer.Ordinal))
                list = subfolder.GetResult(spacesCount, list);

            return list;
        }
    }

    public class DiskTreeTask
    {
        public static List<string> Solve(List<string> input)
        {
            var rootFolder = new Folder("some");
            foreach (var line in input)
            {
                var folders = line.Split('\\');
                var current = rootFolder;
                foreach (var folder in folders)
                    current = current.FindFolder(folder);
            }

            return rootFolder.GetResult(0, new List<string>());
        }
    }
}
