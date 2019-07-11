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
            FileHelper.SelectFiles(ref _files);

            _syncDirectories = false;

            StatusMessage = $"{_files.Count} files selected.";

            OnPropertyChanged("Files");
        }

        public void SelectFolders()
        {
            FileHelper.SelectFolders(ref _files, ref _selectedDirectories);

            _syncDirectories = true;

            StatusMessage = $"{_files.Count} files selected.";

            OnPropertyChanged("Files");
        }

        public void Sync()
        {
            var saveDirectory = FileHelper.SelectDirectory();

            if(saveDirectory == null)
            {
                return;
            }

            StatusMessage = $"Syncing...";

            _progressBar.IsIndeterminate = true;

            if(_syncDirectories)
            {
                foreach(var directory in _selectedDirectories)
                {
                    SyncEngine.RecursivelySyncDirectory(directory, saveDirectory, SyncAll);
                }
            }
            else
            {
                SyncEngine.SyncFiles(_files, saveDirectory, SyncAll);
            }

            _progressBar.IsIndeterminate = false;

            StatusMessage = "Done!";
        }

        public void Clear()
        {
            _files.Clear();

            OnPropertyChanged("Files");

            StatusMessage = "Ready";
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
