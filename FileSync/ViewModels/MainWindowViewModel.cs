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
        private ObservableCollection<IFile> _files;
        public IList<string> Options { get; set; }
        public ObservableCollection<IFile> Files { get => _files; set => _files = value; }
        public DirectoryInfo Directory { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Options = new List<string>();
            Files = new ObservableCollection<IFile>();
        }

        public void SelectFiles()
        {
            FileHelper.SelectFiles(ref _files);

            OnPropertyChanged("Files");
        }

        public void Sync()
        {
            var directory = FileHelper.SelectDirectory();

            Directory = directory;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
