using FileSync.Models;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSync.Helpers
{
    public static class StaticFileHelper
    {
        private static readonly string[] _filter = { ".ini" };
        public static void SelectFiles(ref FileCollection list)
        {
            // Create a file selection dialog that can select multiple files
            var dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            // Show the dialog
            if(dialog.ShowDialog() == true)
            {
                // If the user selected files

                // Clear the master file list
                list.Clear();

                // Load the new files into the master list
                LoadFiles(ref list, dialog.FileNames);

                // Filter ignorable files from the master list
                FilterFiles(ref list);
            }
        }

        public static void SelectFolders(ref FileCollection list, ref List<DirectoryInfo> directories)
        {
            // Create a dialog that allows users to select one or multiple folders
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Multiselect = true,
                ShowHiddenItems = false
            };

            // Show the dialog
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                // If the user made a selection

                // Clear the master file list
                list.Clear();

                // Clear the master directory list
                directories.Clear();

                // Get the list of directories they selected
                var directoryList = dialog.FileNames;

                // For each directory they selected
                foreach(var directory in directoryList)
                {
                    // Get a directory object
                    var newDirectory = new DirectoryInfo(directory);

                    // Add it to the directories list
                    directories.Add(newDirectory);

                    // Recursively load the files from all subdirectories
                    var files = LoadSubdirectories(newDirectory).ToList();

                    // Load the files into the master file collection
                    LoadFiles(ref list, files);
                }

                // Filter ignorable files from the master list
                FilterFiles(ref list);
            }
        }

        public static DirectoryInfo SelectDirectory()
        {
            // Create a new folder opener dialog object
            var dialog = new System.Windows.Forms.FolderBrowserDialog();

            // Show the dialog to the user
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // If the user made a selection

                // Get the pathname for the selected folder
                var dirName = dialog.SelectedPath;

                // Create a directory object
                var directory = new DirectoryInfo(dirName);

                // Return the object
                return directory;
            }

            // If the user did not make a selection, return null
            return null;
        }

        public static void CopyFile(FileInfo file, DirectoryInfo destination)
        {
            // Create the new full path for the file
            var path = $"{destination.FullName}\\{file.Name}";

            // Copy the file object to the new path
            file.CopyTo(path, true);
        }

        private static void LoadFiles(ref FileCollection list, IEnumerable<string> fileNames)
        {
            // Create a temporary list of file objects
            var fileList = new List<FileInfo>();

            // Convert each file name into a file object
            // and add it to the file list
            foreach(var fileName in fileNames)
            {
                var info = new FileInfo(fileName);
                fileList.Add(info);
            }

            // Send this file list to the main LoadFiles method
            LoadFiles(ref list, fileList);
        }

        private static void LoadFiles(ref FileCollection list, IEnumerable<FileInfo> files)
        {
            // For each file object in the enumerable
            foreach(var file in files)
            {
                // Add the file to the master list
                list.Add(file);
            }
        }

        private static IEnumerable<FileInfo> LoadSubdirectories(DirectoryInfo directory, bool first = false)
        {
            // Get the list of files in the current directory
            var files = directory.EnumerateFiles().ToList();

            // For each subdirectory within the current directory
            foreach(var subdir in directory.EnumerateDirectories())
            {
                // Recurse through this algorithm to gather a list of nested files
                var list = LoadSubdirectories(subdir);

                // For each file in the file list retrieved through recursion
                foreach(var item in list)
                {
                    // Add the file to this file list
                    files.Add(item);
                }
            }

            // Return this file list
            return files;
        }

        private static void FilterFiles(ref FileCollection files)
        {
            // Create a list of objects to be removed from the primary list, based on a collection of
            // file extensions to be ignored
            var removable = files.Where(x => _filter.Contains(x.Extension)).ToList();

            // If there are any files matching the filter
            if(removable?.Any() == true)
            {
                // Remove all the 'removable' files from the list
                files.RemoveRange(removable);
            }
        }
    }
}
