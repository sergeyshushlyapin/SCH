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
                Items_Url = request.RequestUri + "api/sch/items",
                Workbox_Url = request.RequestUri + "api/sch/workbox",
                Workflows_Url = request.RequestUri + "api/sch/workflows"
            };
            var sut = new DefaultController { Request = request };

            var actual = sut.Get();

            Assert.Equal(expected, actual, new DefaultUrlsEqualityComparer());
        }
    }
}