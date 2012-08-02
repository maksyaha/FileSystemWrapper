using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using FileSystemWrapper;
using FluentAssertions;
using Xunit;

namespace FileSystemMock.Tests
{
    public class FileSystemMockTests
    {
        [Fact]
        public void root_should_exist()
        {
            IFileSystem fileSystem = FileSystemFluentMock.Create("Root").Build();
            fileSystem.DirectoryWrapper.Exists("Root").Should().BeTrue();
        }

        [Fact]
        public void subdirectory_of_root_should_exist()
        {
            IFileSystem fileSystem = CreateRoot()
                .Dir("Dir1").EndDir()
                .Dir("Dir2")
                    .Dir("SubDir1")
                        .Dir("SubDir2")
                .EndDir()
                .Build();
            fileSystem.DirectoryWrapper.Exists("Root\\Dir1").Should().BeTrue();
            fileSystem.DirectoryWrapper.Exists("Root\\Dir2").Should().BeTrue();
            fileSystem.DirectoryWrapper.Exists("Root\\Dir2\\SubDir1").Should().BeTrue();
            fileSystem.DirectoryWrapper.Exists("Root\\Dir2\\SubDir1\\SubDir2").Should().BeTrue();
        }

        [Fact]
        public void root_should_contain_file()
        {
            IFileSystem fileSystem = CreateRoot()
                .File("Test.txt")
                .Build();
            fileSystem.FileWrapper.Exists("Root\\Test.txt").Should().BeTrue();
        }

        [Fact]
        public void root_subdir_should_contain_file()
        {
            IFileSystem fileSystem = CreateRoot()
                .Dir("Dir1")
                    .Dir("SubDir1")
                        .File("Test.txt")
                    .EndDir()
                .EndDir()
                .Build();
            fileSystem.FileWrapper.Exists("Root\\Dir1\\SubDir1\\Test.txt").Should().BeTrue();            
        }

        [Fact]
        public void DirectoryEnumerateFiles_Path_should_return_files()
        {
            IFileSystem fileSystem = CreateRoot()
                .Dir("Dir1")
                    .File("File1.txt")
                    .File("File2.txt")
                    .Dir("SubDir").EndDir()
                .EndDir()
                .File("File.txt")
                .Build();
            var files = new List<string>(fileSystem.DirectoryWrapper.EnumerateFiles("Root\\Dir1"));
            var rootFiles = new List<string>(fileSystem.DirectoryWrapper.EnumerateFiles("Root"));
            files.Should().BeEquivalentTo(new[] { "Root\\Dir1\\File1.txt", "Root\\Dir1\\File2.txt" });
            rootFiles[0].Should().Be("Root\\File.txt");
        }

        [Fact]
        public void DirectoryEnumerateFiles_all_subdirs()
        {
            var expectedSb = new StringBuilder();
            expectedSb.AppendLine(@"Root\Dir1\File1.txt");
            expectedSb.AppendLine(@"Root\Dir1\File2.txt");
            expectedSb.AppendLine(@"Root\Dir1\SubDir\File5.doc");
            expectedSb.AppendLine(@"Root\Dir1\SubDir\SubDir2\Class.cs");
            expectedSb.AppendLine(@"Root\File.txt");
            expectedSb.AppendLine(@"Root\File.txt");
            expectedSb.AppendLine(@"Root\Dir1\SubDir\SubDir2\Class.cs");
            expectedSb.AppendLine(@"Root\Dir1\SubDir\File5.doc");
            IFileSystem fileSystem = CreateRoot()
                .Dir("Dir1")
                    .File("File1.txt")
                    .File("File2.txt")
                    .Dir("SubDir")
                        .File("File5.doc")
                        .Dir("SubDir2")
                            .File("Class.cs")
                        .EndDir()
                    .EndDir()
                .EndDir()
                .File("File.txt")
                .Build();
            var files = fileSystem.DirectoryWrapper.EnumerateFiles("Root", "*.*", SearchOption.AllDirectories).ToList();
            files.AddRange(fileSystem.DirectoryWrapper.EnumerateFiles("Root", "*.*"));
            files.AddRange(fileSystem.DirectoryWrapper.EnumerateFiles("Root\\Dir1\\SubDir", "*.*", SearchOption.AllDirectories));
            var actualSb = new StringBuilder();
            files.ForEach(f=>actualSb.AppendLine(f));
            actualSb.ToString().Should().Be(expectedSb.ToString());
        }

        [Fact]
        public void FileReadAllText_should_return_file_content()
        {
            string expectedText = "some text";
            IFileSystem fileSystem = CreateRoot()
                .File("File1.txt").Contents(expectedText)
                .Build();
            string actualText = fileSystem.FileWrapper.ReadAllText("Root\\File1.txt");
            actualText.Should().Be(expectedText);
        }

        private FileSystemFluentMock CreateRoot()
        {
            return FileSystemFluentMock.Create("Root");
        }
    }
}
