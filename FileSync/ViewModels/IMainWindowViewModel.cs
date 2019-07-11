using FileSync.Models;
using System.IO;

namespace FileSync.ViewModels
{
    public interface IMainWindowViewModel
    {
        bool SyncAll { get; set; }
        FileCollection<FileInfo> Files { get; set; }
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
