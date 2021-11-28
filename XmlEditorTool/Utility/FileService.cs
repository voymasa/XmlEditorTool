using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.Utility
{
    class FileService
    {
        public static DirectoryInfo[] SetTopLevelDirs(DirectoryInfo toplevelDir, FileInfo xmlFile)
        {
            DirectoryInfo[] temp = new DirectoryInfo[3];
            // assigns two of the 3 subdirs to the RPGBase and SketchEngine directories immediately below the toplevel.
            temp[1] = toplevelDir.GetDirectories("RPGBase", SearchOption.TopDirectoryOnly)[0];
            temp[2] = toplevelDir.GetDirectories("SketchEngine", SearchOption.TopDirectoryOnly)[0];

            // derives the project name from the xml filepath, and assigns it to the array of subdirs
            /*
             * 1. Get the Full path of the xml file
             * 2. Check in the TopDirectoryonly for the directory with the name contained within the xml file fullpath.
             * 3. assign that to the array
             */
            string filepath = xmlFile.FullName;
            int indexOfTopLevel = filepath.IndexOf(toplevelDir.Name);
            string pathStartingAtTopLevel = filepath.Substring(indexOfTopLevel);
            string[] dirName = pathStartingAtTopLevel.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            temp[0] = toplevelDir.GetDirectories(dirName[1], SearchOption.TopDirectoryOnly)[0]; //dirName[1] should correspond to the project folder name immediately beneath the top-level directory

            return temp;
        }
    }
}
