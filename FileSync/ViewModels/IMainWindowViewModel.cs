﻿using FileSync.Models;
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
        ObservableCollection<FileInfo> Files { get; set; }
        string StatusMessage { get; set; }
        int Progress { get; set; }
        int Maximum { get; set; }
        int Minimum { get; set; }
        //DirectoryInfo Directory { get; set; }

        void SelectFiles();

        void Sync();
    }
}
