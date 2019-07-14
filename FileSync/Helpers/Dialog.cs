using FileSync.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace FileSync.Helpers
{
    public class Dialog : IDialog
    {
        public FileCollection PickFiles()
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

        public ICollection<IDirectory> PickFolders()
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

                return collection;
            }

            return null;
        }
    }
}
