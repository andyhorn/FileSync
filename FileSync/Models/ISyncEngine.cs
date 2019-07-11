using System.Collections.Generic;
using System.IO;

namespace FileSync.Models
{
    public interface ISyncEngine
    {
        void SyncFiles(ICollection<FileInfo> files, DirectoryInfo destination, bool syncAll);

        void SyncDirectory(DirectoryInfo directory, DirectoryInfo destination, bool syncAll);
    }
}
