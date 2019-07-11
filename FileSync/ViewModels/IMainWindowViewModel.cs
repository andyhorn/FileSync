using FileSync.Models;
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
    public interface IMainWindowViewModel
    {
        bool SyncAll { get; set; }
        //ObservableCollection<FileInfo> Files { get; set; }
        FileCollection<FileInfo> Files { get; set; }
        string StatusMessage { get; set; }
        int Progress { get; set; }
        int Maximum { get; set; }
        int Minimum { get; set; }

        void SelectFiles();

        void SelectFolders();

        void Sync();

        void Clear();
    }
}
