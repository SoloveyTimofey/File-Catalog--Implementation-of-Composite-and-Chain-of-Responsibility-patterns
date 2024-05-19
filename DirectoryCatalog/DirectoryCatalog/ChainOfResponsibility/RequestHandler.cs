using DirectoryCatalog.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryCatalog.ChainOfResponsibility
{
    public abstract class RequestHandler
    {
        private RequestHandler nextHandler;
        public RequestHandler SetNext(RequestHandler handler)
        {
            nextHandler = handler;
            return handler;
        }

        public abstract void Handle(SearchRequest searchRequest, List<(string, CatalogItem)> results);
    }
}
