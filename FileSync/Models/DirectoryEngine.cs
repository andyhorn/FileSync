using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Models
{
    public class DirectoryEngine : IDirectoryEngine
    {
        public ICollection<IDirectory> GetSubdirectories(IDirectory root)
        {
            var collection = RecurseDirectories(root);

            return collection;
        }

        private ICollection<IDirectory> RecurseDirectories(IDirectory root)
        {
            var subDirs = root.Directories;
            var collection = new Collection<IDirectory>();

            foreach(var dir in subDirs)
            {
                var items = RecurseDirectories(dir);
                
                foreach(var item in items)
                {
                    collection.Add(item);
                }

                collection.Add(dir);
            }

            return collection;
        }

        public void SyncDirectory(IDirectory source, IDirectory destination)
        {
            throw new NotImplementedException();
        }

        public void SyncDirectory(IDirectory source, IDirectory destination, FileCollection files)
        {
            throw new NotImplementedException();
        }
    }
}
