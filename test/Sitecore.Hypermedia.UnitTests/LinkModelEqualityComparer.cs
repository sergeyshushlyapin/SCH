using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class LinkModelEqualityComparer : IEqualityComparer<LinkModel>
    {
        public bool Equals(LinkModel x, LinkModel y)
        {
            return x.Href.Equals(y.Href) && x.Method.Equals(y.Method) && x.Rel.Equals(y.Rel);
        }

        public int GetHashCode(LinkModel obj)
        {
            return 0;
        }
    }
}