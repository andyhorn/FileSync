﻿using FileSync.Models;
using System.Collections.Generic;
using System.IO;

namespace FileSync.ViewModels
{
    public interface IMainWindowViewModel
    {
        bool SyncAll { get; set; }
        FileCollection Files { get; set; }
        ICollection<IDirectory> Directories { get; set; }
        string StatusMessage { get; set; }
        int Progress { get; set; }
        int Maximum { get; set; }
        int Minimum { get; set; }

        void SelectFiles();

        void SelectFolders();

        void Sync();

        void Clear();

        void Remove(FileInfo file);
    }
}
