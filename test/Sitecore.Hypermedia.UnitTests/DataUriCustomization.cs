using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Sitecore.Data;

namespace Sitecore.Hypermedia.UnitTests
{
    public class DataUriCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new MethodInvoker(new GreedyConstructorQuery()),
                    new ExactTypeSpecification(typeof(DataUri))));
        }
    }
}