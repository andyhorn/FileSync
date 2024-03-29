﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FileSync.Models
{
    public class Directory : IDirectory
    {
        private DirectoryInfo _dirInfo;
        public FileCollection Files { get; set; }
        public ICollection<IDirectory> Directories { get; set; }
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
                Directories = new Collection<IDirectory>();

                var dirs = _dirInfo.GetDirectories();

                foreach(var dir in dirs)
                {
                    var newDir = new Directory(dir.FullName);
                    Directories.Add(newDir);
                }

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

        public static ICollection<IDirectory> GetAllContents(IDirectory root)
        {
            var collection = RecurseDirectories(root);

            return collection;
        }

        private static ICollection<IDirectory> RecurseDirectories(IDirectory root)
        {
            var subDirs = root.Directories;
            var collection = new Collection<IDirectory>();

            foreach(var subDir in subDirs)
            {
                var contents = RecurseDirectories(subDir);

                foreach(var item in contents)
                {
                    collection.Add(item);
                }

                collection.Add(subDir);
            }

            return collection;
        }
    }
}
