using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public interface IFile
    {
        string Name { get; set; }
        string Directory { get; set; }
        long Size { get; set; }
        DateTime LastModified { get; set; }
    }
}
