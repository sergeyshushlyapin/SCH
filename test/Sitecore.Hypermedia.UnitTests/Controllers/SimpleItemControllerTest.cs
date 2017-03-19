using System;
using System.Web.Http.Results;
using NSubstitute;
using Sitecore.Hypermedia.Controllers;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Controllers
{
    public class SimpleItemControllerTest
    {
        [Theory, DefaultAutoData]
        public void GetItemReturnsNotFoundIfNoItemFound(
            ISimpleItemService service,
            Guid itemId)
        {
            var sut = new SimpleItemsController(service);
            var result = sut.GetItem(itemId);
            Assert.Null(result);
        }

        [Theory, DefaultAutoData]
        public void GetItemReturnsOkResultWithItemIfFound(
            ISimpleItemService service,
            Guid itemId,
            SimpleItemModel model,
            string name)
        {
            service.GetItem(itemId).Returns(model);
            var sut = new SimpleItemsController(service);
            var result = sut.GetItem(itemId);
            Assert.Same(model, result);
        }

        [Theory, DefaultAutoData]
        public void UpdateTitleReturnsNotFoundIfNoItemFound(
            ISimpleItemService service,
            SimpleItemModel model)
        {
            var sut = new SimpleItemsController(service);
            var result = sut.UpdateTitle(model);
            service.DidNotReceiveWithAnyArgs().Update(model);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory, DefaultAutoData]
        public void UpdateTitleReturnsOkIfItemFound(
            ISimpleItemService service,
            SimpleItemModel model)
        {
            service.GetItem(model.Id).Returns(model);
            var sut = new SimpleItemsController(service);
            var result = sut.UpdateTitle(model);
            service.Received().Update(model);
            Assert.IsType<OkResult>(result);
        }

        [Theory, DefaultAutoData]
        public void ExecuteWorkflowCommandReturnsNotFoundIfNoItemFound(
            ISimpleItemService service,
            Guid itemId,
            string commandId)
        {
            var sut = new SimpleItemsController(service);
            var result = sut.ExecuteWorkflowCommand(itemId, commandId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory, DefaultAutoData]
        public void ExecuteWorkflowCommandReturnsBadRequestIfCommandIsInvalid(
            ISimpleItemService service,
            Guid itemId,
            string commandId,
            SimpleItemModel model)
        {
            service.GetItem(itemId).Returns(model);
            var sut = new SimpleItemsController(service);
            var result = sut.ExecuteWorkflowCommand(itemId, commandId);
            Assert.IsType<BadRequestErrorMessageResult>(result);
        }

        [Theory, DefaultAutoData]
        public void ExecuteWorkflowCommandReturnsOkIfCommandIsExecuted(
            ISimpleItemService service,
            Guid itemId,
            string commandId,
            SimpleItemModel model)
        {
            service.GetItem(itemId).Returns(model);
            service.CanExecuteWorkflowCommand(itemId, commandId)
                .Returns(true);
            var sut = new SimpleItemsController(service);

            var result = sut.ExecuteWorkflowCommand(itemId, commandId);

            Assert.IsType<OkResult>(result);
        }
    }
}
