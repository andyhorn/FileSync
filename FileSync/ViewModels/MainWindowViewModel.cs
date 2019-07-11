using FileSync.Helpers;
using FileSync.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<FileInfo> _files;
        private string _status;
        private ISyncEngine _engine;
        private int _maximum, _minimum, _progress;
        private System.Windows.Controls.ProgressBar _progressBar;
        public bool SyncAll { get; set; }
        public ObservableCollection<FileInfo> Files { get => _files; set => _files = value; }
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
        //public DirectoryInfo Directory { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(System.Windows.Controls.ProgressBar progress)
        {
            _progressBar = progress;
            _progressBar.Minimum = 0;
            _progressBar.Maximum = 100;
            _progressBar.Value = 0;
            //_progressBar.IsIndeterminate = true;

            //Options = new List<SyncOption>();

            //foreach(SyncType type in Enum.GetValues(typeof(SyncType)))
            //{
            //    var option = new SyncOption(type);
            //    Options.Add(option);
            //}

            //SelectedOption = Options[0];

            Files = new ObservableCollection<FileInfo>();
            _engine = new SyncEngine();

            StatusMessage = "Ready";
        }

        public void SelectFiles()
        {
            FileHelper.SelectFiles(ref _files);

            StatusMessage = $"{_files.Count} files selected.";

            OnPropertyChanged("Files");
        }

        public void Sync()
        {
            var directory = FileHelper.SelectDirectory();

            StatusMessage = $"Syncing...";

            _progressBar.IsIndeterminate = true;

            //_engine.Sync(_files, directory, SelectedOption, _progressBar);
            _engine.Sync(_files, directory, SyncAll);

            _progressBar.IsIndeterminate = false;

            StatusMessage = "Done!";
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
