using FileSync.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Helpers
{
    public interface IFileHelper
    {
        void CopyFiles(FileCollection source, FileCollection destination);
    }
}
