using FileSync.Helpers;
using FileSync.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;

namespace FileSync.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel, INotifyPropertyChanged
    {
        private FileCollection _files;
        private string _status;
        private double _maximum, _minimum, _progress;
        private bool _syncDirectories;

        public bool SyncAll { get; set; }
        public FileCollection Files
        {
            get => _files;
            set
            {
                _files = value;
                FilesChanged();
            }
        }
        public ICollection<IDirectory> Directories { get; set; }
        public string StatusMessage
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("StatusMessage");
            }
        }
        public string FileCount
        {
            get => $"{Files.Count} files selected";
        }
        public double Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                OnPropertyChanged("Maximum");
            }
        }
        public double Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                OnPropertyChanged("Minimum");
            }
        }
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Directories = new Collection<IDirectory>();
            _files = new FileCollection();

            Minimum = 0;
            Maximum = 100;

            StatusMessage = "Ready";
        }

        public void SelectFiles()
        {
            // Select one or more files
            var files = DialogFactory.New().PickFiles();

            // If no files were selected (user clicked "Cancel"),
            // do not update any model data
            if(files == null)
            {
                return;
            }

            // Save the selected files to the Files property
            Files = files;

            // Set the sync flag to signify we only have files to copy, not directories
            _syncDirectories = false;
        }

        public void SelectFolders()
        {
            var engine = new DirectoryEngine();

            // Select one or more directories from which to copy files
            var selected = DialogFactory.New().PickFolders();

            foreach(var selection in selected)
            {
                var directories = engine.GetSubdirectories(selection);

                foreach(var directory in directories)
                {
                    Directories.Add(directory);
                }
            }

            // Set the flag to signify we have directories to copy along with their files
            _syncDirectories = true;
        }

        public void Sync()
        {
            // Use the file helper to locate a save directory
            var saveDirectory = StaticFileHelper.SelectDirectory();

            // If no directory was chosen, return early
            if(saveDirectory == null)
            {
                return;
            }

            // Set an appropriate status message
            StatusMessage = $"Syncing...";

            // If we are syncing directories
            if(_syncDirectories)
            {
                // Loop through each chosen directory
                foreach(var directory in Directories)
                {
                    // Recursively synchronize it and its subdirectories with the destination
                    SyncEngine.RecursivelySyncDirectory(_files, new DirectoryInfo(directory.FullPath), saveDirectory, SyncAll);
                }

                SyncEngine.RecursivelyRemoveIfEmpty(saveDirectory);
            }
            else
            {
                // Otherwise, synchronize all chosen files with the destination
                SyncEngine.SyncFiles(_files, saveDirectory, SyncAll);
            }

            // Set an appropriate status message
            StatusMessage = "Done!";
        }

        public void Clear()
        {
            _files.Clear();

            FilesChanged();

            StatusMessage = "Ready";
        }

        public void Remove(FileInfo file)
        {
            Files.Remove(file);

            FilesChanged();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FilesChanged()
        {
            OnPropertyChanged("Files");
            OnPropertyChanged("FileCount");
        }
    }
}
