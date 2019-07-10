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
        public static void SelectFiles(ref ObservableCollection<IFile> list)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if(dialog.ShowDialog() == true)
            {
                //var list = new ObservableCollection<IFile>();

                list.Clear();

                var fileNames = dialog.FileNames;

                foreach(var name in fileNames)
                {
                    var fileInfo = new FileInfo(name);

                    var file = new Models.File
                    {
                        Name = fileInfo.Name,
                        Size = fileInfo.Length,
                        Directory = fileInfo.DirectoryName,
                        LastModified = fileInfo.LastWriteTime
                    };

                    list.Add(file);
                }

                //return list;
            }

            //return null;
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
    }
}
