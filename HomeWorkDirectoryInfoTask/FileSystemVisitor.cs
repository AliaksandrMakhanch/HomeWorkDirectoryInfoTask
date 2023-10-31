using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemView 
{
    public class FileSystemVisitor
    {
        private readonly DirectoryInfo _rootDirectory;
        private readonly FilterDelegate _filter;

        public delegate bool FilterDelegate(FileSystemInfo item);

        public FileSystemVisitor(string rootDirectoryPath) : this(rootDirectoryPath, null)
        {
        }

        public FileSystemVisitor(string rootDirectoryPath, FilterDelegate filter)
        {
            _rootDirectory = new DirectoryInfo(rootDirectoryPath);
            _filter = filter ?? DefaultFilter;
        }

        private static bool DefaultFilter(FileSystemInfo item)
        {
            return true;
        }

        public IEnumerable<FileSystemInfo> Traverse()
        {
            Stack<DirectoryInfo> directories = new Stack<DirectoryInfo>();
            directories.Push(_rootDirectory);

            while (directories.Count > 0)
            {
                DirectoryInfo currentDirectory = directories.Pop();

                FileSystemInfo[] items;
                try
                {
                    items = currentDirectory.EnumerateFileSystemInfos().ToArray();
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }

                foreach (FileSystemInfo item in items)
                {
                    if (item is DirectoryInfo itemDirectory)
                    {
                        directories.Push(itemDirectory);
                    }

                    if (_filter(item))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}