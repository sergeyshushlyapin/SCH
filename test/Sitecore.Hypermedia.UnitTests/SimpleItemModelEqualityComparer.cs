using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class SimpleItemModelEqualityComparer : IEqualityComparer<SimpleItemModel>
    {
        public bool Equals(SimpleItemModel x, SimpleItemModel y)
        {
            if (ReferenceEquals(x, y))
                return true;

            return Equals(x.Id, y.Id)
                   && Equals(x.Language, y.Language)
                   && Equals(x.Version, y.Version)
                   && Equals(x.Name, y.Name)
                   && Equals(x.Title, y.Title)
                   && Equals(x.Workflow, y.Workflow);
        }

        public int GetHashCode(SimpleItemModel obj)
        {
            return 0;
        }
    }
}