using Ploeh.AutoFixture.Xunit2;

namespace Sitecore.Hypermedia.UnitTests
{
    internal class InlineDefaultAutoDataAttribute : InlineAutoDataAttribute
    {
        public InlineDefaultAutoDataAttribute(params object[] values)
            : base(new DefaultAutoDataAttribute(), values)
        {
        }
    }
}