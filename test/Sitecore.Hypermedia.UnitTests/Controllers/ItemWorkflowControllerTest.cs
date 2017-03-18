using System;
using System.Web.Http.Results;
using NSubstitute;
using Sitecore.Hypermedia.Controllers;
using Sitecore.Hypermedia.Model;
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
            Assert.Null(result);
        }

        [Theory, DefaultAutoData]
        public void GetItemReturnsOkResultWithItemIfFound(
            IItemWorkflowService service,
            Guid itemId,
            ItemModel model,
            string name)
        {
            service.GetItem(itemId).Returns(model);
            var sut = new ItemWorkflowController(service);
            var result = sut.GetItem(itemId);
            Assert.Same(model, result);
        }

        [Theory, DefaultAutoData]
        public void UpdateTitleReturnsNotFoundIfNoItemFound(
            IItemWorkflowService service,
            ItemModel model)
        {
            var sut = new ItemWorkflowController(service);
            var result = sut.UpdateTitle(model);
            service.DidNotReceiveWithAnyArgs().Update(model);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory, DefaultAutoData]
        public void UpdateTitleReturnsOkIfItemFound(
            IItemWorkflowService service,
            ItemModel model)
        {
            service.GetItem(model.Id).Returns(model);
            var sut = new ItemWorkflowController(service);
            var result = sut.UpdateTitle(model);
            service.Received().Update(model);
            Assert.IsType<OkResult>(result);
        }

        [Theory, DefaultAutoData]
        public void ExecuteWorkflowCommandReturnsNotFoundIfNoItemFound(
            IItemWorkflowService service,
            Guid itemId,
            string commandId)
        {
            var sut = new ItemWorkflowController(service);
            var result = sut.ExecuteWorkflowCommand(itemId, commandId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory, DefaultAutoData]
        public void ExecuteWorkflowCommandReturnsBadRequestIfCommandIsInvalid(
            IItemWorkflowService service,
            Guid itemId,
            string commandId,
            ItemModel model)
        {
            service.GetItem(itemId).Returns(model);
            var sut = new ItemWorkflowController(service);
            var result = sut.ExecuteWorkflowCommand(itemId, commandId);
            Assert.IsType<BadRequestErrorMessageResult>(result);
        }

        [Theory, DefaultAutoData]
        public void ExecuteWorkflowCommandReturnsOkIfCommandIsExecuted(
            IItemWorkflowService service,
            Guid itemId,
            string commandId,
            ItemModel model)
        {
            service.GetItem(itemId).Returns(model);
            service.CanExecuteWorkflowCommand(itemId, commandId)
                .Returns(true);
            var sut = new ItemWorkflowController(service);

            var result = sut.ExecuteWorkflowCommand(itemId, commandId);

            Assert.IsType<OkResult>(result);
        }
    }
}
