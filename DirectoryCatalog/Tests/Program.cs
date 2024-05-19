using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectoryCatalog.ChainOfResponsibility;
using DirectoryCatalog.Composite;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryItem rootDirectory = new DirectoryItem("C:", null);

            DirectoryItem officeInstruments = new DirectoryItem("OfficeInstruments", rootDirectory);
            FileItem word = new FileItem("Microsoft Word", officeInstruments, ItemType.Office);
            FileItem excel = new FileItem("Microsoft Excel", officeInstruments, ItemType.Office);

            DirectoryItem gamingAndMultimedia = new DirectoryItem("Gaming&Multimedia", rootDirectory);

            DirectoryItem games = new DirectoryItem("Games", gamingAndMultimedia);

            DirectoryItem multimedia = new DirectoryItem("Multimedia", gamingAndMultimedia);
            FileItem myVideo = new FileItem("MyVideo", multimedia, ItemType.Multimedia);

            SaveToFile("results.bin", rootDirectory);
            DirectoryItem directoryItem = LoadFromFile("results.bin");

            SearchingHandler searchingHandler = new SearchingHandler(rootDirectory);
            List<(string, CatalogItem)> results = new List<(string, CatalogItem)>();
            searchingHandler.Handle(new SearchRequest("OfficeInstruments", null), results);

            for (int i = 0; i < results.Count(); i++)
            {
                Console.WriteLine(results[i].Item1);
            }

            Console.ReadLine();
        }

        private static void SaveToFile(string filePath, DirectoryItem root)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, root);
            }
        }

        private static DirectoryItem LoadFromFile(string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream  = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (DirectoryItem)formatter.Deserialize(stream);
            }
        }

        private static void RestoreParentReferences(CatalogItem item, DirectoryItem parent)
        {
            item.Owner = parent;
            if (item is DirectoryItem directory)
            {
                foreach (var child in directory.Children)
                {
                    RestoreParentReferences(child, directory);
                }
            }
        }
    }
}
