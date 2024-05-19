using DirectoryCatalog.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCatalog.ChainOfResponsibility
{
    public class SearchingHandler : RequestHandler
    {
        private readonly CatalogItem item;
        public SearchingHandler(CatalogItem item)
        {
            this.item = item;
        }

        //results argument will be populated with searching results
        public override void Handle(SearchRequest searchRequest, List<(string, CatalogItem)> results)
        {
            if (item.Name.ToLower().Contains(searchRequest.Name.ToLower()) && searchRequest.Type==null?true:item.Type==searchRequest.Type)
            {
                results.Add(item.GetPathToCurrent());
            }
            if (item is DirectoryItem directory)
            {
                foreach (var children in directory.Children)
                {
                    SearchingHandler handler = new SearchingHandler(children);
                    handler.SetNext(handler).Handle(searchRequest, results);
                }
            }
        }
    }
}
