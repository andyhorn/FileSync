using FileSync.Models;
using System.Threading.Tasks;

namespace FileSync.Helpers
{
    public static class AsyncFileHelper
    {
        public static async Task CopyFiles(FileCollection source, FileCollection destination)
        {
            await Task.Run(() =>
            {
                Worker(source, destination);
            });
        }

        private static void Worker(FileCollection source, FileCollection destination)
        {
            foreach(var file in source)
            {
                destination.Add(file);
            }
        }
    }
}
