using System;
using System.IO;
using System.Linq;

namespace FileSync.Models
{
    public class Directory : IDirectory
    {
        private DirectoryInfo _dirInfo;
        public FileCollection Files { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool Exists { get; private set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastAccess { get; set; }
        public DateTime LastModified { get; set; }

        public Directory(string fullPath)
        {
            FullPath = fullPath;
            _dirInfo = new DirectoryInfo(FullPath);

            if(_dirInfo.Exists)
            {
                Files = new FileCollection(_dirInfo.GetFiles().ToArray());

                Exists = true;

                Name = _dirInfo.Name;

                DateCreated = _dirInfo.CreationTime;
                LastAccess = _dirInfo.LastAccessTime;
                LastModified = _dirInfo.LastWriteTime;
            }
            else
            {
                Exists = false;
            }
        }

        public Directory(DirectoryInfo directory)
        {
            _dirInfo = directory;
            Exists = _dirInfo.Exists;
            Name = _dirInfo.Name;
            FullPath = _dirInfo.FullName;

            if(Exists)
            {
                Files = new FileCollection(_dirInfo.GetFiles().ToArray());
                DateCreated = _dirInfo.CreationTime;
                LastAccess = _dirInfo.LastAccessTime;
                LastModified = _dirInfo.LastWriteTime;
            }
        }

        public void Create()
        {
            if(!Exists)
            {
                _dirInfo.Create();
                Exists = _dirInfo.Exists;
            }
        }

        public void Delete()
        {
            if(Exists)
            {
                _dirInfo.Delete();
                _dirInfo = null;
            }
        }
    }
}
