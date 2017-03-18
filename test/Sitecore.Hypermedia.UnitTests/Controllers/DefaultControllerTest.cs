using System.Net.Http;
using Sitecore.Hypermedia.Controllers;
using Sitecore.Hypermedia.Model;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Controllers
{
    public class DefaultControllerTest
    {
        [Theory, DefaultAutoData]
        public void GetReturnsDefaultLinks(
            HttpRequestMessage request)
        {
            var expected = new DefaultUrls
            {
                ItemsUrl = request.RequestUri + "api/sch/items",
                WorkboxUrl = request.RequestUri + "api/sch/workbox"
            };
            request.SetConfiguration(new DefaultHttpConfiguration());
            var sut = new DefaultController { Request = request };

            var actual = sut.Get();

            Assert.Equal(expected, actual, new DefaultUrlsEqualityComparer());
        }
    }
}