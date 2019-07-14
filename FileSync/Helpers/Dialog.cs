using FileSync.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace FileSync.Helpers
{
    public class Dialog : IDialog
    {
        public FileCollection OpenFiles()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true
            };

            if(dialog.ShowDialog() == true)
            {
                var collection = new FileCollection();

                foreach(var file in dialog.FileNames)
                {
                    var fileInfo = new FileInfo(file);
                    collection.Add(fileInfo);
                }

                return collection;
            }

            return null;
        }

        public ICollection<IDirectory> OpenFolders()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Multiselect = true,
                ShowHiddenItems = false
            };

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var collection = new Collection<IDirectory>();

                //var dir = new Models.Directory(dialog.SelectedPath);

                //collection.Add(dir);

                foreach(var pathname in dialog.FileNames)
                {
                    var dir = new Models.Directory(pathname);

                    collection.Add(dir);
                }

                //collection.Add(new Models.Directory(dialog.))

                return collection;
            }

            return null;
        }

        public IDirectory SaveFolder()
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true
            };

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                var selectedPath = dialog.SelectedPath;
                return new Models.Directory(selectedPath);
            }

            return null;
        }
    }
}
