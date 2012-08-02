using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using FileSystemWrapper;
using NSubstitute;
using System.Linq;

namespace FileSystemMock
{
    internal class FileSystemMockBuilder
    {
        private IDirectory directoryMock;
        private IFile fileMock;

        public IFileSystem Build(DirectoryItem root)
        {
            var fileSystem = Substitute.For<IFileSystem>();
            directoryMock = Substitute.For<IDirectory>();
            fileMock = Substitute.For<IFile>();
            fileSystem.DirectoryWrapper.Returns(directoryMock);
            fileSystem.FileWrapper.Returns(fileMock);

            MockDir(root);

            return fileSystem;
        }

        private void MockDir(DirectoryItem item)
        {
            directoryMock.Exists(item.FullPath).Returns(true);
            var dirFiles = item.Children.OfType<FileItem>().ToList();
            List<DirectoryItem> subDirs = item.Children.OfType<DirectoryItem>().ToList();
            subDirs.ForEach(MockDir);
            dirFiles.ForEach(f=>MockFile(item, f));
            List<string> allFiles = dirFiles.Select(f => Path.Combine(item.FullPath, f.Name)).ToList();
            directoryMock.EnumerateFiles(item.FullPath).Returns(allFiles);
            directoryMock.EnumerateFiles(item.FullPath, Arg.Any<string>())
                .Returns
                ((callInfo)=>
                     {
                         var searchPattern = (string)callInfo.Args()[1];
                         var regex = new Regex(WildcardToRegex(searchPattern));
                         return allFiles.Where(f => regex.IsMatch(f));
                     }
                );
            directoryMock.EnumerateFiles(item.FullPath, Arg.Any<string>(), SearchOption.AllDirectories)
                .Returns(c =>
                {
                    var files = new List<string>();
                    var searchPattern = (string)c.Args()[1];
                    var regex = new Regex(WildcardToRegex(searchPattern));
                    GetSubDirs(item).ForEach(sd => files.AddRange(directoryMock.EnumerateFiles(sd.FullPath, searchPattern)
                        .Where(f => regex.IsMatch(f))));
                    files.AddRange(allFiles.Where(f => regex.IsMatch(f)));
                    return files;
                });
            directoryMock.EnumerateDirectories(item.FullPath).Returns(subDirs.Select(_=>_.FullPath).ToList());
        }

        private void MockFile(DirectoryItem parent, FileItem fileItem)
        {
            string filePath = Path.Combine(parent.FullPath, fileItem.Name);
            fileMock.Exists(filePath).Returns(true);
            fileMock.ReadAllText(filePath).Returns(fileItem.Contents);
        }

        private List<DirectoryItem> GetSubDirs(DirectoryItem item)
        {
            var subDirs = new List<DirectoryItem>();
            foreach (DirectoryItem dirItem in item.Children.OfType<DirectoryItem>())
            {
                subDirs.Add(dirItem);
                subDirs.AddRange(GetSubDirs(dirItem));
            }
            return subDirs;
        }

        private string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).
            Replace("\\*", ".*").
            Replace("\\?", ".") + "$";
        }
    }
}
