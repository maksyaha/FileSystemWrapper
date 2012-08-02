using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileSystemWrapper
{
    public interface IFileSystemEntity
    {
        bool Exists(string path);
        FileSystemInfo GetInfo(string path);
    }
}
