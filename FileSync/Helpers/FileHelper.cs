using FileSync.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Helpers
{
    public static class FileHelper
    {
        public static void SelectFiles(ref ObservableCollection<FileInfo> list)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if(dialog.ShowDialog() == true)
            {
                list.Clear();

                var fileNames = dialog.FileNames;

                foreach(var name in fileNames)
                {
                    var info = new FileInfo(name);
                    list.Add(info);
                }
            }
        }

        public static DirectoryInfo SelectDirectory()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();

            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var dirName = dialog.SelectedPath;

                var directory = new DirectoryInfo(dirName);

                return directory;
            }

            return null;
        }

        public static void CopyFile(FileInfo file, DirectoryInfo destination)
        {
            var path = $"{destination.FullName}\\{file.Name}";

            file.CopyTo(path, true);
        }
    }
}
