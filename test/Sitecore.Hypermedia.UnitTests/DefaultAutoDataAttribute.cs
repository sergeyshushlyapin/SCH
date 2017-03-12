using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit2;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Hypermedia.UnitTests
{
    internal class DefaultAutoDataAttribute : AutoDataAttribute
    {
        public DefaultAutoDataAttribute()
        {
            Fixture
                .Customize(new AutoNSubstituteCustomization())
                .Customize(new NSubstituteForTypeCustomization(typeof(Database)))
                .Customize(new NSubstituteForTypeCustomization(typeof(Item)))
                .Customize(new NSubstituteForTypeCustomization(typeof(ItemState)));
        }
    }
}
