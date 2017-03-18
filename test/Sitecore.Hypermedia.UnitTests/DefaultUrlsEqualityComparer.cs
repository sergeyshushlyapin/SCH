using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class DefaultUrlsEqualityComparer : IEqualityComparer<DefaultUrls>
    {
        public bool Equals(DefaultUrls x, DefaultUrls y)
        {
            return x.ItemsUrl.Equals(y.ItemsUrl)
                && x.WorkboxUrl.Equals(y.WorkboxUrl);
        }

        public int GetHashCode(DefaultUrls obj)
        {
            return 0;
        }
    }
}