using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCatalog.Composite
{
    [Serializable]
    public class DirectoryItem : CatalogItem
    {
        public List<CatalogItem> Children = new List<CatalogItem>();
        public DirectoryItem(string name, DirectoryItem owner) : base(name, owner, ItemType.Directory)
        {
            
        }

        public void Add(CatalogItem item)
        {
            ChangeOwner(item, this);
            Children.Add(item);
        }

        public void Remove(CatalogItem item)
        {
            Children.Remove(item);
        }
    }
}
