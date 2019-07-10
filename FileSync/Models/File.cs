using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public class File : IFile, INotifyPropertyChanged
    {
        private string _name, _directory;
        private long _size;
        private DateTime _lastModified;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(this, "Name");
            }
        }
        public string Directory
        {
            get => _directory;
            set
            {
                _directory = value;
                OnPropertyChanged(this, "Directory");
            }
        }
        public long Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged(this, "Size");
            }
        }
        public DateTime LastModified
        {
            get => _lastModified;
            set
            {
                _lastModified = value;
                OnPropertyChanged(this, "LastModified");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
