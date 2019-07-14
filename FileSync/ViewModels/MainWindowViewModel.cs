using FileSync.Helpers;
using FileSync.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace FileSync.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel, INotifyPropertyChanged
    {
        private FileCollection files;
        private string status;
        private bool syncDirectories;
        private ISyncEngine syncEngine;
        private Overwrite overwrite;

        public Overwrite Overwrite
        {
            get => overwrite;
            set
            {
                overwrite = value;
                syncEngine.Overwrite = Overwrite;
            }
        }
        public FileCollection Files
        {
            get => files;
            set
            {
                files = value;
                FilesChanged();
            }
        }
        public ICollection<IDirectory> Directories { get; set; }
        public string StatusMessage
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged("StatusMessage");
            }
        }
        public string FileCount
        {
            get => $"{Files.Count} files selected";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Directories = new Collection<IDirectory>();
            Files = new FileCollection();

            syncEngine = new SyncEngine();

            StatusMessage = "Ready";
        }

        public void SelectFiles()
        {
            // Select one or more files
            var files = DialogFactory.New().OpenFiles();

            // If no files were selected (e.g., user clicked "Cancel"),
            // do not update any model data
            if(files == null)
            {
                return;
            }

            // Save the selected files to the Files property,
            // this will also trigger the update notification
            Files = files;

            // Set the sync flag to signify we only have files to copy, not directories
            syncDirectories = false;
        }

        public void SelectFolders()
        {
            // Select one or more directories from which to copy files
            var selected = DialogFactory.New().OpenFolders();

            // For each directory selected by the user
            foreach(var selection in selected)
            {
                Directories.Add(selection);
                // Recursively scan the contents of the directory 
                // and all of its subdirectories
                //var directories = Models.Directory.GetAllContents(selection);

                //// Add each subdirectory to the master directory list
                //foreach(var directory in directories)
                //{
                //    Directories.Add(directory);
                //}
            }

            var files = new FileCollection();

            foreach(var dir in Directories)
            {
                files.AddRange(dir.Files);

                var subdirs = Models.Directory.GetAllContents(dir);

                foreach(var subdir in subdirs)
                {
                    files.AddRange(subdir.Files);
                }
            }

            Files = files;

            // Set the flag to signify we have directories to copy along with their files
            syncDirectories = true;
        }

        public void Sync()
        {
            // Select a save directory
            var saveDirectory = DialogFactory.New().SaveFolder();

            // If no directory was chosen, return early
            if(saveDirectory == null)
            {
                return;
            }

            // Set the synchronization settings
            //syncEngine.Overwrite = Overwrite;

            // Set an appropriate status message
            StatusMessage = $"Syncing...";

            // If we are synchronizing one or more directories
            if(syncDirectories)
            {
                // Loop through each directory
                foreach(var directory in Directories)
                {
                    // Synchronize it with the selected destination directory,
                    // using the list of files as a filter
                    var path = syncEngine.GetPath(saveDirectory.FullPath, directory.Name);
                    var destination = new Models.Directory(path);
                    syncEngine.Sync(destination, directory, Files);
                }
            }
            else
            {
                // Otherwise, synchronize all files to the destination directory
                syncEngine.Sync(saveDirectory, Files);
            }

            // Set an appropriate status message
            StatusMessage = "Done!";
        }

        public void Clear()
        {
            // Remove all files from the collection
            files.Clear();

            // Signal that the files have changed
            FilesChanged();

            // Update the status message
            StatusMessage = "Ready";
        }

        public void Remove(FileInfo file)
        {
            // Remove the file from the collection
            Files.Remove(file);

            // Signal that the file collection has changed
            FilesChanged();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FilesChanged()
        {
            // Signal that the file collection and file count has changed
            OnPropertyChanged("Files");
            OnPropertyChanged("FileCount");
        }
    }
}
