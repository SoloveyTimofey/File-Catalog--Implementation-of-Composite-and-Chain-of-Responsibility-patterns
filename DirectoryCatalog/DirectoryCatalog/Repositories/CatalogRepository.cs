using DirectoryCatalog.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DirectoryCatalog.StaticFiles;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace DirectoryCatalog.Repositories
{
    public class CatalogRepository
    {
        private CatalogStorage catalogStorage { get; set; }
        private static CatalogRepository _instance;

        protected CatalogRepository(CatalogStorage catalogStorage)
        {
            this.catalogStorage = catalogStorage;
        }

        //Singleton
        public static CatalogRepository GetInstance()
        {
            if (_instance==null)
            {
                _instance = new CatalogRepository(new CatalogStorage(LoadFromFile(Paths.CatalogPath)));
            }

            return _instance;
        }

        public DirectoryItem GetCatalogItems()
        {
            return catalogStorage.DirectoryItem;
        }

        public void SaveCatalog(DirectoryItem directoryItem)
        {
            SaveToFile(Paths.CatalogPath, directoryItem);
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
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (DirectoryItem)formatter.Deserialize(stream);
            }
        }

        //public void AddCatalogItem(CatalogItem catalogItem)
        //{
        //    catalogStorage.CatalogItems.Add(catalogItem);
        //    string json = JsonConvert.SerializeObject(catalogStorage.CatalogItems);
        //    File.WriteAllText(Paths.CatalogPath, json);
        //}

        //private static List<CatalogItem> GetCatalogItemsFromJson()
        //{
        //    string json = File.ReadAllText(Paths.CatalogPath);

        //    if (json.Length==0)
        //    {
        //        json = "[]";
        //    }

        //    return JsonConvert.DeserializeObject<List<CatalogItem>>(json);
        //}
    }
}
