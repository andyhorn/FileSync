using FileSync.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileSync.Helpers
{
    public static class FileHelper
    {
        private static readonly string[] _filter = { ".ini" };
        public static void SelectFiles(ref FileCollection<FileInfo> list)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if(dialog.ShowDialog() == true)
            {
                LoadFiles(ref list, dialog.FileNames);
            }
        }

        public static void SelectFolders(ref FileCollection<FileInfo> list)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Multiselect = true,
                ShowHiddenItems = false
            };

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var directory = new DirectoryInfo(dialog.FileName);

                var files = LoadSubdirectories(directory).ToList();

                LoadFiles(ref list, files);
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

        private static void LoadFiles(ref FileCollection<FileInfo> list, IEnumerable<string> fileNames)
        {
            list.Clear();

            foreach(var file in fileNames)
            {
                var info = new FileInfo(file);

                list.Add(info);
            }

            FilterFiles(ref list);
        }

        private static void LoadFiles(ref FileCollection<FileInfo> list, IEnumerable<FileInfo> files)
        {
            list.Clear();

            foreach(var file in files)
            {
                list.Add(file);
            }

            FilterFiles(ref list);
        }

        private static IEnumerable<FileInfo> LoadSubdirectories(DirectoryInfo directory, bool first = false)
        {
            var files = directory.EnumerateFiles().ToList();

            foreach(var subdir in directory.EnumerateDirectories())
            {
                var list = LoadSubdirectories(subdir);

                foreach(var item in list)
                {
                    files.Add(item);
                }
            }

            return files;
        }

        private static void FilterFiles(ref FileCollection<FileInfo> files)
        {
            //var newList = new FileCollection<FileInfo>();

            //foreach(var file in files)
            //{
            //    if(!_filter.Contains(file.Extension))
            //    {
            //        newList.Add(file);
            //    }
            //}

            //return newList;

            var removable = files.Where(x => _filter.Contains(x.Extension)).ToList();

            if(removable?.Any() == true)
            {
                files.RemoveRange(removable);
                //foreach(var file in removable)
                //{
                //    files.Remove(file);
                //}
            }
        }
    }
}
