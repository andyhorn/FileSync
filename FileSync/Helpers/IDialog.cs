using FileSync.Models;
using System.Collections.Generic;

namespace FileSync.Helpers
{
    public interface IDialog
    {
        ICollection<IDirectory> PickFolders();
        FileCollection PickFiles();
    }
}
