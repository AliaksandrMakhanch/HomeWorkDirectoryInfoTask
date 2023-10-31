using System;

namespace FileSystemView 
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootDirectoryPath = @"C:\Users\Aliaksandr_Makhnach\source\repos\FileSystemHomeWork\";

            Console.WriteLine("Common list of all files and subfolders:");
            var fileSystemVisitor = new FileSystemVisitor(rootDirectoryPath);
            foreach (var item in fileSystemVisitor.Traverse())
            {
                Console.WriteLine(item.FullName);
            }

            Console.WriteLine("\nJust subfolders:");
            FileSystemVisitor.FilterDelegate filter = (item) => item is DirectoryInfo;
            var filteredFileSystemVisitor = new FileSystemVisitor(rootDirectoryPath, filter);
            foreach (var item in filteredFileSystemVisitor.Traverse())
            {
                Console.WriteLine(item.FullName);
            }
        }
    }
}