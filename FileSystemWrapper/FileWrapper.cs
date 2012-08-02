using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileSystemWrapper
{
    public class FileWrapper : IFile
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public FileSystemInfo GetInfo(string fileName)
        {
            return new FileInfo(fileName);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
