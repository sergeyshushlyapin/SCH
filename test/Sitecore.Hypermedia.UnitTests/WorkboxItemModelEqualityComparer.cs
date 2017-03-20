using System.Collections.Generic;
using System.Linq;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class WorkboxItemModelEqualityComparer : IEqualityComparer<WorkboxItemModel>
    {
        public bool Equals(WorkboxItemModel x, WorkboxItemModel y)
        {
            return x.Name.Equals(y.Name)
                   && x.Url.Equals(y.Url)
                   && x.Commands.SequenceEqual(y.Commands, new LinkModelEqualityComparer());
        }

        public int GetHashCode(WorkboxItemModel obj)
        {
            return 0;
        }
    }
}