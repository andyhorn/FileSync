using FileSync.Models;

namespace FileSync.Helpers
{
    public static class FileHelperFactory
    {
        public static FileHelper New()
        {
            return new FileHelper();
        }
    }
    public class FileHelper : IFileHelper
    {
        public void CopyFiles(FileCollection source, FileCollection destination)
        {
            foreach(var file in source)
            {
                destination.Add(file);
            }
        }
    }
}
