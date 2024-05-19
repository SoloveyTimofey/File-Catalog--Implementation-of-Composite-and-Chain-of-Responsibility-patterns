using DirectoryCatalog.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCatalog.Repositories
{
    [Serializable]
    public class CatalogStorage
    {
        public DirectoryItem DirectoryItem { get; set; }
        public CatalogStorage(DirectoryItem directoryItem)
        {
            this.DirectoryItem = directoryItem;
        }
    }
}
