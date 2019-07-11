using FileSync.Helpers;
using FileSync.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace FileSync.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel, INotifyPropertyChanged
    {
        private FileCollection<FileInfo> _files;
        private List<DirectoryInfo> _selectedDirectories;
        private string _status;
        private int _maximum, _minimum, _progress;
        private System.Windows.Controls.ProgressBar _progressBar;
        private bool _syncDirectories;
        public bool SyncAll { get; set; }
        public FileCollection<FileInfo> Files { get => _files; set => _files = value; }
        public string StatusMessage
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("StatusMessage");
            }
        }
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                OnPropertyChanged("Maximum");
            }
        }
        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                OnPropertyChanged("Minimum");
            }
        }
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(System.Windows.Controls.ProgressBar progress)
        {
            _progressBar = progress;
            _progressBar.Minimum = 0;
            _progressBar.Maximum = 100;
            _progressBar.Value = 0;

            _selectedDirectories = new List<DirectoryInfo>();

            _files = new FileCollection<FileInfo>();

            StatusMessage = "Ready";
        }

        public void SelectFiles()
        {
            // Use the file helper to select one or more files
            FileHelper.SelectFiles(ref _files);

            // Set the sync flag to signify we only have files to copy, not directories
            _syncDirectories = false;

            // Set an appropriate status message
            StatusMessage = $"{_files.Count} files selected.";

            OnPropertyChanged("Files");
        }

        public void SelectFolders()
        {
            // Use the file helper to select one or more directories from which to copy files
            FileHelper.SelectFolders(ref _files, ref _selectedDirectories);

            // Set the flag to signify we have directories to copy along with their files
            _syncDirectories = true;

            // Set an appropriate status message
            StatusMessage = $"{_files.Count} files selected.";

            OnPropertyChanged("Files");
        }

        public void Sync()
        {
            // Use the file helper to locate a save directory
            var saveDirectory = FileHelper.SelectDirectory();

            // If no directory was chosen, return early
            if(saveDirectory == null)
            {
                return;
            }

            // Set an appropriate status message
            StatusMessage = $"Syncing...";

            // Activate the progress bar
            _progressBar.IsIndeterminate = true;

            // If we are syncing directories
            if(_syncDirectories)
            {
                // Loop through each chosen directory
                foreach(var directory in _selectedDirectories)
                {
                    // Recursively synchronize it and its subdirectories with the destination
                    SyncEngine.RecursivelySyncDirectory(directory, saveDirectory, SyncAll);
                }
            }
            else
            {
                // Otherwise, synchronize all chosen files with the destination
                SyncEngine.SyncFiles(_files, saveDirectory, SyncAll);
            }

            // Deactivate the progress bar
            _progressBar.IsIndeterminate = false;

            // Set an appropriate status message
            StatusMessage = "Done!";
        }

        public void Clear()
        {
            _files.Clear();

            OnPropertyChanged("Files");

            StatusMessage = "Ready";
        }

        public void Remove(FileInfo file)
        {
            Files.Remove(file);

            OnPropertyChanged("Files");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
