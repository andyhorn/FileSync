namespace FileSync.Models
{
    public enum Overwrite
    {
        All,
        New,
        None
    }
    public interface ISyncEngine
    {
        Overwrite Overwrite { get; set; }
        void Sync(IDirectory destination, IDirectory source, FileCollection files);
        void Sync(IDirectory destination, FileCollection files);
    }
}
