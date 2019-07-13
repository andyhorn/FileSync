using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public interface IDirectory
    {
        FileCollection Files { get; set; }
        string Name { get; set; }
        string FullPath { get; set; }
        DateTime DateCreated { get; set; }
        DateTime LastAccess { get; set; }
        DateTime LastModified { get; set; }
        bool Exists { get; }
        void Create();
        void Delete();
    }
}
