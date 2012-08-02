using System;
using System.Collections.Generic;

namespace FileSystemMock
{
    internal class DirectoryItem : FileSystemItemBase
    {
        public string Path { get; private set; }
        public string FullPath
        {
            get { return System.IO.Path.Combine(Path, Name); }
        }
        private readonly Lazy<List<FileSystemItemBase>> lazyChildren = new Lazy<List<FileSystemItemBase>>(()=>new List<FileSystemItemBase>());
        public List<FileSystemItemBase> Children
        {
            get { return lazyChildren.Value; }
        }
        public DirectoryItem(string path, string name)
            : base(name)
        {
            Path = path;
        }

        public override string ToString()
        {
            return string.Format("Path:{0} Name:{1}", Path, Name);
        }
    }
}
