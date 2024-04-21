using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CollectionManagmentSystem
{
    public static class FileManager
    {
        public static string GetDataFolderPath()
        {
            var dataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CollectionManagementSystem");
            Trace.WriteLine($"Generated data folder path: {dataFolderPath}");
            return dataFolderPath;
        }

        public static List<Collection> LoadCollections(string dataFolderPath)
        {
            var collections = new List<Collection>();

            Trace.WriteLine("LoadCollections method called"); 

            if (!Directory.Exists(dataFolderPath))
                Directory.CreateDirectory(dataFolderPath);

            Trace.WriteLine($"Loading collections from path: {dataFolderPath}");

            var files = Directory.GetFiles(dataFolderPath, "*.txt");
            foreach (var file in files)
            {
                var collectionName = Path.GetFileNameWithoutExtension(file);
                var lines = File.ReadAllLines(file);
                var items = new List<string>();
                var description = string.Empty;

                foreach (var line in lines)
                {
                    if (line.StartsWith("#Description: "))
                    {
                        description = line.Substring("#Description: ".Length);
                    }
                    else
                    {
                        items.Add(line);
                    }
                }

                collections.Add(new Collection { Name = collectionName, Description = description, Items = items });
            }

            Trace.WriteLine($"Collections loaded from path: {dataFolderPath}");
            return collections;
        }

        public static void SaveCollections(List<Collection> collections, string dataFolderPath)
        {
            Trace.WriteLine("SaveCollections method called"); 

            if (!Directory.Exists(dataFolderPath))
                Directory.CreateDirectory(dataFolderPath);

            Trace.WriteLine($"Saving collections to path: {dataFolderPath}");

            foreach (var collection in collections)
            {
                var filePath = Path.Combine(dataFolderPath, $"{collection.Name}.txt");
                File.WriteAllLines(filePath, collection.Items);

                File.AppendAllText(filePath, $"{Environment.NewLine}#Description: {collection.Description}");
            }

            Trace.WriteLine($"Collections saved to path: {dataFolderPath}");
        }
    }
}
