using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public class File : IFile
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
    }
}
