using FileSync.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Helpers
{
    public interface IFileEngine
    {
        void SyncAllFiles(FileCollection files, IDirectory destination);
        void SyncNewFiles(FileCollection files, IDirectory destination);
        void FileCopy(FileCollection source, FileCollection destination);
    }
}
