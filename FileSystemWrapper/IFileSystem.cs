namespace FileSystemWrapper
{
    public interface IFileSystem
    {
        IDirectory DirectoryWrapper { get; }
        IFile FileWrapper { get; }
    }
}