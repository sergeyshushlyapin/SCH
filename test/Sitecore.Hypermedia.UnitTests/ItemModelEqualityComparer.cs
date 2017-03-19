using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class ItemModelEqualityComparer : IEqualityComparer<ItemModel>
    {
        public bool Equals(ItemModel x, ItemModel y)
        {
            if (ReferenceEquals(x, y))
                return true;

            return Equals(x.Name, y.Name)
                   && Equals(x.Title, y.Title)
                   && Equals(x.Url, y.Url);
        }

        public int GetHashCode(ItemModel obj)
        {
            return 0;
        }
    }
}