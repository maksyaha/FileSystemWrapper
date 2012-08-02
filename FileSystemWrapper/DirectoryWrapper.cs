using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileSystemWrapper
{
    public class DirectoryWrapper : IDirectory
    {
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public FileSystemInfo GetInfo(string path)
        {
            return new DirectoryInfo(path);
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            return Directory.EnumerateFiles(path);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern);
        }

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(path, searchPattern, searchOption);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }


        public IEnumerable<string> EnumerateDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            return Directory.EnumerateDirectories(path, searchPattern);
        }

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(path, searchPattern, searchOption);
        }
    }
}
