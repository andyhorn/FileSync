using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public interface IDirectoryEngine
    {
        void SyncDirectory(IDirectory source, IDirectory destination);
        void SyncDirectory(IDirectory source, IDirectory destination, FileCollection files);
        ICollection<IDirectory> GetSubdirectories(IDirectory root);
    }
}
