using System.Net.Http;
using Ploeh.AutoFixture;

namespace Sitecore.Hypermedia.UnitTests
{
    public class HttpRequestMessageCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var request = fixture.Freeze<HttpRequestMessage>();
            request.SetConfiguration(new DefaultHttpConfiguration());
        }
    }
}