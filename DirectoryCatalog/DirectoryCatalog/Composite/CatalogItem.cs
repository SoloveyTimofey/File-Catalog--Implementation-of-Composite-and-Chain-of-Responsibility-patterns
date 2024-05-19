using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCatalog.Composite
{
    [Serializable]
    public abstract class CatalogItem
    {
        public string Name;
        public DirectoryItem Owner;
        public ItemType Type;

        public CatalogItem(string name, DirectoryItem owner, ItemType type)
        {
            this.Name = name;

            if (owner!=null)
            {
                owner.Add(this);
            }
            this.Owner = owner;

            Type = type;
        }

        public void ChangeOwner(CatalogItem targetItem, DirectoryItem newOwner)
        {
            targetItem.Owner?.Remove(this);
            targetItem.Owner = newOwner;
        }

        public (string, CatalogItem) GetPathToCurrent()
        {
            List<CatalogItem> parents = new List<CatalogItem>();
            CatalogItem currentParent = this.Owner;
            while (true)
            {
                if (currentParent == null)
                {
                    break;
                }      
                
                parents.Add(currentParent);
                currentParent = currentParent.Owner;
            }

            return (BuildPath(parents, this), this);
        }

        private string BuildPath(List<CatalogItem> parents, CatalogItem targetItem)
        {
            parents.Reverse();

            StringBuilder path = new StringBuilder();
            for (int i = 0; i < parents.Count; i++)
            {
                if (i==0)
                {
                    path.Append(parents[i].Name);
                }
                else
                {
                    path.Append("/" + parents[i].Name);
                }
            }
            path.Append("/"+targetItem.Name);

            return path.ToString();
        }
    }

    public enum ItemType
    {
        Directory,
        System,
        Office,
        Gaming,
        Multimedia
    }

}
