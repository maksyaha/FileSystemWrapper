using System.IO;

namespace FileSystemWrapper
{
    public interface IFile : IFileSystemEntity
    {
        void WriteAllText(string path, string contents);
        string ReadAllText(string path);
    }
}