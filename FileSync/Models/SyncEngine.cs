using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private IEnumerable<FileInfo> GetFilteredList(FileCollection original, FileCollection filter)
        {
            var filterList = filter.ToList().Select(f => f.Name);
            var originalList = original.ToList();

            var filteredList = originalList.Where(x => filterList.FirstOrDefault(f => x.Name.Equals(f)) != null);

            return filteredList;
        }
        public Overwrite Overwrite { get; set; }
        /// <summary>
        /// Synchronizes a source directory with a destination directory,
        /// only copying those files that exist in the file collection.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="files"></param>
        public void Sync(IDirectory destination, IDirectory source, FileCollection files = null)
        {
            // Get a list of all files in source directory
            var sourceContents = source.Files;

            // Get a list of the subdirectories
            var subdirectories = source.Directories;

            // Create a filtered list of items to copy based on the contents
            // of the files collection filter (if applicable)
            var toCopy = files != null
                ? GetFilteredList(sourceContents, files)
                : sourceContents;

            // Check if the destination contains the source directory
            var dest = new Directory(GetPath(destination.FullPath, source.Name));

            if(!dest.Exists)
            {
                dest.Create();
            }

            // Loop through the toCopy list and copy each item to the destination
            foreach(var item in toCopy)
            {
                var path = GetPath(dest.FullPath, item.Name);
                Copy(item, path);
            }

            if(subdirectories.Any())
            {
                // For each subdirectory
                foreach(var subdirectory in subdirectories)
                {
                    // Get the new path within the destination
                    var path = GetPath(dest.FullPath, subdirectory.Name);

                    // Create a directory object
                    var newDestination = new Directory(path);

                    // If the folder doesn't exist, create it
                    //if(!newDestination.Exists)
                    //{
                    //    newDestination.Create();
                    //}

                    // Recurse through this algorithm
                    Sync(newDestination, subdirectory, files);
                }
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
