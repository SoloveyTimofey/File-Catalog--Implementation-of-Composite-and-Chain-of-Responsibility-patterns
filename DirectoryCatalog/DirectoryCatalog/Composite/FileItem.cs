using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCatalog.Composite
{
    [Serializable]
    public class FileItem : CatalogItem
    {
        public FileItem(string name, DirectoryItem owner, ItemType type) : base(name, owner, type)
        {
        }
    }
}
