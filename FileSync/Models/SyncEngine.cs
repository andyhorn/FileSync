using FileSync.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public class SyncEngine : ISyncEngine
    {
        public void Sync(ICollection<FileInfo> files, DirectoryInfo destination, SyncOption option)
        {
            switch(option.Type)
            {
                case SyncType.CopyAll:
                    SyncAll(files, destination);
                    break;
                case SyncType.CopyNew:
                    SyncNew(files, destination);
                    break;
                default:
                    break;
            }
        }

        private void SyncAll(ICollection<FileInfo> files, DirectoryInfo destination)
        {
            foreach(var f in files)
            {
                //var fileInfo = new FileInfo(f.Name);
                FileHelper.CopyFile(f, destination);
            }
        }

        private void SyncNew(ICollection<FileInfo> files, DirectoryInfo destination)
        {
            //var destinationFiles = destination.EnumerateFiles();
            var currentFiles = destination.GetFiles();
            //var subdirs = destination.EnumerateDirectories();

            foreach(var f in files)
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
                    FileHelper.CopyFile(f, destination);
                }
            }
        }
    }
}
