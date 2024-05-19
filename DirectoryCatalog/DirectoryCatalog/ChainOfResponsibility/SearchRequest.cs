using DirectoryCatalog.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCatalog.ChainOfResponsibility
{
    public class SearchRequest
    {
        public string Name { get; set; }
        public ItemType? Type { get; set; }
        public SearchRequest(string name, ItemType? type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
