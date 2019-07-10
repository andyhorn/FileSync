using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public enum SyncOption
    {
        CopyAll,
        CopyNew
    }
    public interface ISyncEngine
    {
        void Sync(ICollection<IFile> files, DirectoryInfo destination, SyncOption option);
    }
}
