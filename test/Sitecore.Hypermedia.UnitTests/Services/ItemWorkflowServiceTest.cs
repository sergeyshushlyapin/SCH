using System;
using NSubstitute;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Hypermedia.Services;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Services
{
    public class ItemWorkflowServiceTest
    {
        [Theory, DefaultAutoData]
        public void GetItemReturnsItem(
            Database database,
            Guid itemId,
            Item expected)
        {
            database.GetItem(new ID(itemId)).Returns(expected);
            var sut = new ItemWorkflowService(database);
            var actual = sut.GetItem(itemId);
            Assert.Same(expected, actual);
        }
    }
}
