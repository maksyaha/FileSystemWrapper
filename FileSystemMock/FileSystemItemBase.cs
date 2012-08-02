namespace FileSystemMock
{
    internal abstract class FileSystemItemBase
    {
        public string Name { get; private set; }

        protected FileSystemItemBase(string name)
        {
            Name = name;
        }
    }
}
