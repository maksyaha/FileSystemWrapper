namespace FileSystemMock
{
    internal class FileItem : FileSystemItemBase
    {
        public string Contents { get; set; }

        public FileItem(string name) : base(name)
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
