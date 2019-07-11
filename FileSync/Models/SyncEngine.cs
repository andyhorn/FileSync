using FileSync.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FileSync.Models
{
    public class SyncEngine : ISyncEngine
    {
        private ICollection<FileInfo> _files;
        private DirectoryInfo _directory;
        public void Sync(ICollection<FileInfo> files, DirectoryInfo destination, bool syncAll)
        {
            _files = files;
            _directory = destination;

            if(syncAll)
            {
                SyncAll();
            }
            else
            {
                SyncNew();
            }
        }

        private void SyncAll()
        {
            foreach(var f in _files)
            {
                FileHelper.CopyFile(f, _directory);
            }
        }

        private void SyncNew()
        {
            var currentFiles = _directory.GetFiles();

            foreach(var f in _files)
            {
                var existingFile = currentFiles.FirstOrDefault(x => x.Name == f.Name);
                var copy = true;

                if(existingFile != null)
                {
                    if(existingFile.LastWriteTime >= f.LastWriteTime)
                    {
                        copy = false;
                    }
                }

                if(copy)
                {
                    FileHelper.CopyFile(f, _directory);
                }
            }
        }
    }
}
