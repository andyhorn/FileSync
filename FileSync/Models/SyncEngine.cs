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
        public void Sync(ICollection<IFile> files, DirectoryInfo destination, SyncOption option)
        {
            switch(option)
            {
                case SyncOption.CopyAll:
                    SyncAll(files, destination);
                    break;
                case SyncOption.CopyNew:
                    SyncNew(files, destination);
                    break;
                default:
                    break;
            }
        }

        private void SyncAll(ICollection<IFile> files, DirectoryInfo destination)
        {
            foreach(var f in files)
            {
                var fileInfo = new FileInfo(f.Name);
                FileHelper.CopyFile(fileInfo, destination);
            }
        }

        private void SyncNew(ICollection<IFile> files, DirectoryInfo destination)
        {
            var destinationFiles = destination.EnumerateFiles();
            var subdirs = destination.EnumerateDirectories();

            foreach(var f in files)
            {
                var info = new FileInfo(f.Name);

                if(!destinationFiles.Contains(info) ||
                    destinationFiles.First(x => x.Name == info.Name).LastWriteTime < f.LastModified)
                {
                    FileHelper.CopyFile(info, destination);
                }
            }
        }
    }
}
