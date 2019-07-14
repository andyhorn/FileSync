using FileSync.Models;
using System.Collections.Generic;
using System.IO;

namespace FileSync.ViewModels
{
    public interface IMainWindowViewModel
    {
        Overwrite Overwrite { get; set; }
        FileCollection Files { get; set; }
        ICollection<IDirectory> Directories { get; set; }
        string StatusMessage { get; set; }
        string FileCount { get; }

        void SelectFiles();

        void SelectFolders();

        void Sync();

        void Clear();

        void Remove(FileInfo file);

        void FilesChanged();
    }
}
