using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class DefaultUrlsEqualityComparer : IEqualityComparer<DefaultUrls>
    {
        public bool Equals(DefaultUrls x, DefaultUrls y)
        {
            return x.Items_Url.Equals(y.Items_Url)
                && x.Workbox_Url.Equals(y.Workbox_Url)
                && x.Workflows_Url.Equals(y.Workflows_Url);
        }

        public int GetHashCode(DefaultUrls obj)
        {
            return 0;
        }
    }
}