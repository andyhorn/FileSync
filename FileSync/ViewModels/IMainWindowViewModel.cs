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
        IList<string> Options { get; set; }
        //IList<FileInfo> Files { get; set; }
        //IList<IFile> Files { get; set; }
        ObservableCollection<IFile> Files { get; set; }

        void SelectFiles();

        void Sync();
    }
}
