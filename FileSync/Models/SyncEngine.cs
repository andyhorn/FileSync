using FileSync.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSync.Models
{
    public class SyncEngine : ISyncEngine
    {
        private ICollection<FileInfo> _files;
        private DirectoryInfo _directory;
        private DirectoryInfo _destination;
        public void SyncFiles(ICollection<FileInfo> files, DirectoryInfo destination, bool syncAll)
        {
            _files = files;
            _destination = destination;

            if(syncAll)
            {
                SyncAllFiles();
            }
            else
            {
                SyncNewFiles();
            }
        }

        public void SyncDirectory(DirectoryInfo directory, DirectoryInfo destination, bool syncAll)
        {
            _directory = directory;
            _destination = destination;

            RecursivelySyncDirectory(directory, destination, syncAll);

            //switch(syncAll)
            //{
            //    case true:
            //        RecursivelySyncDirectory(director)
            //}

            //if(syncAll)
            //{
            //    SyncAllDirectories();
            //}
            //else
            //{
            //    SyncNewDirectories();
            //}
        }

        private void SyncAllFiles()
        {
            foreach(var f in _files)
            {
                FileHelper.CopyFile(f, _destination);
            }
        }

        private void SyncNewFiles()
        {
            var currentFiles = _destination.GetFiles();

            foreach(var f in _files)
            {
                var existingFile = currentFiles.FirstOrDefault(x => x.Name == f.Name);
                var copy = true;

                if(existingFile != null)
                {
                    if(existingFile.LastWriteTime >= f.LastWriteTime)
                    {
                        copy = false;
                    }
                }

                if(copy)
                {
                    FileHelper.CopyFile(f, _destination);
                }
            }
        }

        //private void SyncAllDirectories()
        //{
        //    RecursivelySyncDirectory(_directory, _destination, true);
        //}

        //private void SyncNewDirectories()
        //{
        //    RecursivelySyncDirectory(_directory, _destination, false);
        //}

        private void RecursivelySyncDirectory(DirectoryInfo directory, DirectoryInfo destination, bool forceOverwrite)
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

            // For each file in this current directory
            foreach(var file in directory.GetFiles())
            {
                // Search for an existing file in the destination
                var existingFile = newDirectory.GetFiles().FirstOrDefault(x => x.Name == file.Name);

                // If no file currently exists OR we are copying everything OR the existing file is older than the current file,
                // copy it to the destination directory
                if(existingFile == null || forceOverwrite || existingFile.LastWriteTime < file.LastWriteTime)
                {
                    FileHelper.CopyFile(file, newDirectory);
                }
            }

            // Set the current directory as the "destination" directory
            var newDestination = new DirectoryInfo(newDirectoryPath);

            // For each of the current directory's subdirectories, recurse through this algorithm,
            // using this directory as the new 'destination' directory
            foreach(var subDirectory in directory.GetDirectories())
            {
                RecursivelySyncDirectory(subDirectory, newDestination, forceOverwrite);
            }
        }
    }
}
