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
        IList<SyncOption> Options { get; set; }
        SyncOption SelectedOption { get; set; }
        //IList<FileInfo> Files { get; set; }
        //IList<IFile> Files { get; set; }
        ObservableCollection<IFile> Files { get; set; }
        DirectoryInfo Directory { get; set; }

        void SelectFiles();

        void Sync();
    }
}
