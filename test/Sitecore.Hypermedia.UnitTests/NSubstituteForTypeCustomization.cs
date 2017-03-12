using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Kernel;

namespace Sitecore.Hypermedia.UnitTests
{
    internal class NSubstituteForTypeCustomization : ICustomization
    {
        private readonly Type _type;

        public NSubstituteForTypeCustomization(Type type)
        {
            _type = type;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(
                new FilteringSpecimenBuilder(
                    new MethodInvoker(new NSubstituteMethodQuery()),
                    new ExactTypeSpecification(_type)));
        }
    }
}