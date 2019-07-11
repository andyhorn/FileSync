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
        private SyncOption _sync;
        private ProgressBar _progressBar;
        public void Sync(ICollection<FileInfo> files, DirectoryInfo destination, SyncOption option, ProgressBar progressBar)
        {
            _files = files;
            _directory = destination;
            _sync = option;
            _progressBar = progressBar;

            _progressBar.IsIndeterminate = true;

            switch(option.Type)
            {
                case SyncType.CopyAll:
                    //await Task.Run(new Action(SyncAll));
                    //SyncAll(files, destination, option);
                    SyncAll();
                    break;
                case SyncType.CopyNew:
                    //SyncNew(files, destination, progressBar);
                    SyncNew();
                    //await Task.Run(new Action(SyncNew));
                    break;
                default:
                    break;
            }

            _progressBar.IsIndeterminate = false;
        }

        //private async void SyncAll(ICollection<FileInfo> files, DirectoryInfo destination, ProgressBar progressBar)
        private void SyncAll()
        {
            //_progressBar.Maximum = _files.Count;
            //_progressBar.Value = 0;

            //_progressBar.IsIndeterminate = true;

            foreach(var f in _files)
            {
                FileHelper.CopyFile(f, _directory);
                //_progressBar.Value += 1;
            }

            //_progressBar.IsIndeterminate = false;
        }

        //private async void SyncNew(ICollection<FileInfo> files, DirectoryInfo destination, ProgressBar progressBar)
        private void SyncNew()
        {
            //var destinationFiles = destination.EnumerateFiles();
            //var currentFiles = destination.GetFiles();
            var currentFiles = _directory.GetFiles();
            //var subdirs = destination.EnumerateDirectories();

            //_progressBar.Maximum = _files.Count;
            //_progressBar.Value = 0;

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

                //_progressBar.Value += 1;
            }
        }
    }
}
