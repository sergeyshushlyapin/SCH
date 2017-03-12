using Ploeh.AutoFixture.Xunit2;
using Sitecore.Hypermedia.Model;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Model
{
    public class WorkflowModelTest
    {
        [Theory, AutoData]
        public void EqualsWithNullReturnsFalse(WorkflowModel sut)
        {
            var result = sut.Equals(null);
            Assert.False(result);
        }

        [Theory, AutoData]
        public void EqualsWithSameReturnsTrue(WorkflowModel sut)
        {
            var result = sut.Equals(sut);
            Assert.True(result);
        }
    }
}