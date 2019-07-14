﻿using System.Linq;

namespace FileSync.Models
{
    /// <summary>
    /// Provides concrete implementation of the ISyncEngine interface
    /// </summary>
    public class SyncEngine : ISyncEngine
    {
        public SyncEngine()
        {
            Overwrite = Overwrite.All;
        }

        public SyncEngine(Overwrite overwrite)
        {
            Overwrite = overwrite;
        }
        public Overwrite Overwrite { get; set; }
        /// <summary>
        /// Synchronizes a source directory with a destination directory,
        /// only copying those files that exist in the file collection.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="files"></param>
        public void Sync(IDirectory destination, IDirectory source, FileCollection files)
        {
            // Get a list of all files in source directory
            var sourceContents = source.Files;

            // Create a filtered list of items to copy based on the contents
            // of the files collection
            var toCopy = sourceContents.Where(x => files.Contains(x));

            // Loop through the toCopy list and copy each item to the destination
            foreach(var item in toCopy)
            {
                var path = GetPath(destination.FullPath, item.Name);
                Copy(item, path);
            }
        }

        /// <summary>
        /// Synchronizes a source directory with a destination directory,
        /// copying over all files.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public void Sync(IDirectory destination, IDirectory source)
        {
            // Get the list of files in the source directory
            var fileList = source.Files;

            // Loop through the list and copy each file
            // to the destination directory
            foreach(var file in fileList)
            {
                var path = GetPath(destination.FullPath, file.Name);
                Copy(file, path);
            }
        }

        /// <summary>
        /// Synchronizes a collection of files to a destination directory
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="files"></param>
        public void Sync(IDirectory destination, FileCollection files)
        {
            foreach(var file in files)
            {
                var path = GetPath(destination.FullPath, file.Name);
                Copy(file, path);
            }
        }

        private string GetPath(string directory, string file)
        {
            return System.IO.Path.Combine(directory, file);
        }

        private void Copy(System.IO.FileInfo file, string path)
        {
            var destinationFile = new System.IO.FileInfo(path);
            var fileExists = destinationFile.Exists;

            switch(Overwrite)
            {
                case Overwrite.All:
                    file.CopyTo(path, true);
                    break;
                case Overwrite.None:
                    if(!fileExists)
                    {
                        file.CopyTo(path);
                    }
                    break;
                case Overwrite.New:
                    var isNewer = destinationFile.LastWriteTime >= file.LastWriteTime;
                    if(!isNewer)
                    {
                        file.CopyTo(path, true);
                    }
                    break;
            }
        }
    }
}
