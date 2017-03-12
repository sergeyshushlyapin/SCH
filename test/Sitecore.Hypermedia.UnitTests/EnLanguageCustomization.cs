using Ploeh.AutoFixture;
using Sitecore.Globalization;

namespace Sitecore.Hypermedia.UnitTests
{
    internal class EnLanguageCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Inject(Language.Parse("en"));
        }
    }
}