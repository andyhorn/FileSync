using FileSync.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSync.Models
{
    public static class SyncEngine
    {
        public static void SyncFiles(FileCollection files, DirectoryInfo destination, bool syncAll)
        {
            // Choose the correct mode of synchronization
            if(syncAll)
            {
                // Forcibly overwrite all existing files in destination
                SyncAllFiles(files, destination);
            }
            else
            {
                // Only copy files that are newer than their counterparts in the destination
                SyncNewFiles(files, destination);
            }
        }

        private static void SyncAllFiles(FileCollection files, DirectoryInfo destination)
        {
            // Copy each file in the file list to the destination
            foreach(var f in files)
            {
                StaticFileHelper.CopyFile(f, destination);
            }
        }

        private static void SyncNewFiles(FileCollection files, DirectoryInfo destination)
        {
            // Get the list of files in the base directory
            var currentFiles = destination.GetFiles();

            // FileInfo object for use during the loop
            FileInfo existingFile = null;

            // For each file
            foreach(var f in files)
            {
                // Check for an existing file
                existingFile = currentFiles.FirstOrDefault(x => x.Name == f.Name);

                // If no duplicate file exists OR the current file is newer than the existing file, copy
                if(existingFile == null || f.LastWriteTime > existingFile.LastWriteTime)
                {
                    StaticFileHelper.CopyFile(f, destination);
                }
            }
        }

        public static void RecursivelySyncDirectory(FileCollection files, DirectoryInfo directory, DirectoryInfo destination, bool forceOverwrite)
        {
            // Get the current directory's name
            var directoryName = directory.Name;

            // Get the path to the destination directory
            var destinationPath = destination.FullName;

            // Combine the current directory with the destination to get the full path
            var newDirectoryPath = Path.Combine(destinationPath, directoryName);

            // Create the new directory object
            var newDirectory = new DirectoryInfo(newDirectoryPath);

            // If this directory does not exist in the destination, create it
            if(!newDirectory.Exists)
            {
                newDirectory.Create();
            }

            var dirFiles = directory.GetFiles();

            // For each file in this current directory
            foreach(var file in dirFiles)
            {
                // If the file was removed from the master file list, do not copy it
                if(files.FirstOrDefault(x => x.FullName == file.FullName) == null)
                {
                    continue;
                }

                // Search for an existing file in the destination
                var existingFile = newDirectory.GetFiles().FirstOrDefault(x => x.Name == file.Name);

                // If no file currently exists OR we are copying everything OR the existing file is older than the current file,
                // copy it to the destination directory
                if(existingFile == null || forceOverwrite || existingFile.LastWriteTime < file.LastWriteTime)
                {
                    StaticFileHelper.CopyFile(file, newDirectory);
                }
            }

            // Set the current directory as the "destination" directory
            var newDestination = new DirectoryInfo(newDirectoryPath);

            // For each of the current directory's subdirectories, recurse through this algorithm,
            // using this directory as the new 'destination' directory
            foreach(var subDirectory in directory.GetDirectories())
            {
                RecursivelySyncDirectory(files, subDirectory, newDestination, forceOverwrite);
            }
        }

        public static void RecursivelyRemoveIfEmpty(DirectoryInfo directory)
        {
            if(RecursiveIsEmpty(directory))
            {
                directory.Delete();
            }
        }

        public static bool RecursiveIsEmpty(DirectoryInfo directory)
        {
            // Get a list of the subdirectories
            var subdirs = directory.GetDirectories().ToList();

            // Get a list of files within this directory
            var files = directory.GetFiles().Where(x => x.Extension != ".ini").ToList();

            // Create a list of directories to remove
            var toDelete = new List<DirectoryInfo>();

            // For each subdirectory in this directory
            foreach(var subdir in subdirs)
            {
                // Recurse through this algorithm
                // If the subdirectory is empty
                if(RecursiveIsEmpty(subdir))
                {
                    // Add it to the list of directories to remove
                    toDelete.Add(subdir);
                }
            }

            // Once all subdirectories have been scanned,
            // loop through the tracking list
            foreach(var dir in toDelete)
            {
                // Delete all empty subdirectories
                dir.Delete();
            }

            // Return true if this directory is empty
            return !directory.GetDirectories().ToList().Any() && !files.Any();
        }
    }
}
