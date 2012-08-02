using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileSystemWrapper;

namespace FileSystemMock
{
    public class FileSystemFluentMock
    {
        internal DirectoryItem Root { get; private set; }
        private readonly List<DirectoryItem> dirChain = new List<DirectoryItem>();
        private FileSystemItemBase ActiveItem { get; set; }

        public static FileSystemFluentMock Create(string root)
        {
            return new FileSystemFluentMock(root);
        }

        protected FileSystemFluentMock(string root)
        {
            Dir(root);
            Root = dirChain.First();
        }

        public FileSystemFluentMock Dir(string name)
        {
            string path = Path.Combine(dirChain.Select(_ => _.Name).ToArray());
            var directoryItem = new DirectoryItem(path, name);
            UpdateActiveItem(directoryItem);
            return this;
        }

        public FileSystemFluentMock EndDir()
        {
            if (dirChain.Count > 0)
                dirChain.RemoveAt(dirChain.Count - 1);
            return this;
        }

        public FileSystemFluentMock File(string name)
        {
            var fileItem = new FileItem(name);
            UpdateActiveItem(fileItem);
            return this;
        }

        public FileSystemFluentMock Contents(string contents)
        {
            var fileItem = ActiveItem as FileItem;
            if(fileItem==null)
                throw new InvalidOperationException("Contents method could be applied only to File item.");
            fileItem.Contents = contents;
            return this;
        }

        public IFileSystem Build()
        {
            return new FileSystemMockBuilder().Build(Root);
        }

        private void UpdateActiveItem(FileSystemItemBase item)
        {
            var currentDir = dirChain.LastOrDefault();
            if (currentDir != null)
                currentDir.Children.Add(item);
            var directoryItem = item as DirectoryItem;
            if (directoryItem != null)
                dirChain.Add(directoryItem);
            ActiveItem = item;
        }
    }
}
