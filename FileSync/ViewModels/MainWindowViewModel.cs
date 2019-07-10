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
        //private IList<FileInfo> _fileList;
        public IList<string> Options { get; set; }
        //public IList<FileInfo> Files { get => _fileList; set
        //    {
        //        _fileList = value;
        //        OnPropertyChanged("Files");
        //    }
        //}

        //public IList<IFile> Files { get; set; }
        public ObservableCollection<IFile> Files { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Options = new List<string>();
            //Files = new List<FileInfo>();
            //Files = new List<IFile>();
            Files = new ObservableCollection<IFile>();
        }

        public void SelectFiles()
        {
            var fileDialog = new OpenFileDialog()
            {
                Multiselect = true
            };

            if(fileDialog.ShowDialog() == true)
            {
                var fileNames = fileDialog.FileNames;

                foreach(var name in fileNames)
                {
                    var info = new FileInfo(name);

                    var file = new Models.File
                    {
                        Name = info.Name,
                        LastModified = info.LastWriteTime,
                        Directory = info.DirectoryName,
                        Size = info.Length
                    };

                    Files.Add(file);

                    //Files.Add(info);
                }
            }

            OnPropertyChanged("Files");
        }

        public void Sync()
        {
            throw new NotImplementedException();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
