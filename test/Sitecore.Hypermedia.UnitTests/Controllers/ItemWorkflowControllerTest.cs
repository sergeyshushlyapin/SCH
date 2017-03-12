using System;
using System.Web.Http.Results;
using NSubstitute;
using Sitecore.Data.Items;
using Sitecore.Hypermedia.Controllers;
using Sitecore.Hypermedia.Services;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Controllers
{
    public class ItemWorkflowControllerTest
    {
        [Theory, DefaultAutoData]
        public void GetItemReturnsNotFoundIfNoItemFound(
            IItemWorkflowService service,
            Guid itemId)
        {
            var sut = new ItemWorkflowController(service);
            var result = sut.GetItem(itemId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory, DefaultAutoData]
        public void GetItemReturnsItemIfFound(
            IItemWorkflowService service,
            Guid itemId,
            Item item)
        {
            service.GetItem(itemId).Returns(item);
            var sut = new ItemWorkflowController(service);
            var result = sut.GetItem(itemId);
            Assert.IsType<OkResult>(result);
        }
    }
}
