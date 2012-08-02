using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSystemWrapper
{
    public class FileSystem : IFileSystem
    {
        public IDirectory DirectoryWrapper
        {
            get { return new DirectoryWrapper();}
        }

        public IFile FileWrapper
        {
            get { return new FileWrapper();}
        }
    }
}
