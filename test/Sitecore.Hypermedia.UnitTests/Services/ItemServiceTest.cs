using System;
using NSubstitute;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Hypermedia.Services;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Services
{
    public class ItemServiceTest
    {
        [Theory, DefaultAutoData]
        public void GetItemReturnsNullIfNotFound(
            Database database,
            Guid itemId)
        {
            var sut = new ItemService(database);
            var actual = sut.GetItem(itemId);
            Assert.Null(actual);
        }

        [Theory, DefaultAutoData]
        public void GetItemReturnsItemIfFound(
            Database database,
            Guid itemId,
            Item expected)
        {
            database.GetItem(new ID(itemId)).Returns(expected);
            var sut = new ItemService(database);
            var actual = sut.GetItem(itemId);
            Assert.Same(expected, actual);
        }
    }
}