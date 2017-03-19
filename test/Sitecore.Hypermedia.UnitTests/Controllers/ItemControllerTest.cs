using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Hypermedia.Controllers;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;
using Xunit;
using Version = Sitecore.Data.Version;

namespace Sitecore.Hypermedia.UnitTests.Controllers
{
    public class ItemControllerTest
    {
        [Theory, DefaultAutoData]
        public void GetReturnsEmptyListIfIfNoItemsFound(
            [Frozen] HttpRequestMessage request,
            IItemService service)
        {
            var sut = new ItemsController(service) { Request = request };
            var result = sut.Get();
            Assert.Empty(result);
        }

        [Theory, DefaultAutoData]
        public void GetReturnsItemModelsIfFound(
            [Frozen] HttpRequestMessage request,
            IItemService service,
            Item item1,
            Item item2,
            Language language,
            Version version,
            string name1,
            string name2,
            string title1,
            string title2)
        {
            item1.Language.Returns(language);
            item1.Version.Returns(version);
            item1.Name.Returns(name1);
            item1["Title"].Returns(title1);

            item2.Language.Returns(language);
            item2.Version.Returns(version);
            item2.Name.Returns(name2);
            item2["Title"].Returns(title2);

            service.GetContentItems()
                .Returns(new List<Item> { item1, item2 });
            var sut = new ItemsController(service) { Request = request };
            var result = sut.Get();
            Assert.Equal(2, result.Count());
        }

        [Theory, DefaultAutoData]
        public void GetReturnsNullIfNoItemFound(
            IItemService service,
            Guid itemId)
        {
            var sut = new ItemsController(service);
            var result = sut.Get(itemId);
            Assert.Null(result);
        }

        [Theory, DefaultAutoData]
        public void GetReturnsValidItemModel(
            [Frozen] HttpRequestMessage request,
            IItemService service,
            Item item,
            Language language,
            Version version,
            string name,
            string title,
            UrlHelper urlHelper)
        {
            item.Language.Returns(language);
            item.Version.Returns(version);
            item.Name.Returns(name);
            item["Title"].Returns(title);
            var url = urlHelper.Link("SchItem", new
            {
                itemId = item.ID.Guid,
                language = language.Name,
                version = version.Number
            });
            var expected = new ItemModel
            {
                Name = name,
                Title = title,
                Url = url
            };
            service.GetItem(item.ID.Guid).Returns(item);
            var sut = new ItemsController(service) { Request = request };

            var actual = sut.Get(item.ID.Guid);

            Assert.Equal(expected, actual, new ItemModelEqualityComparer());
        }
    }
}