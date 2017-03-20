using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Results;
using NSubstitute;
using Sitecore.Hypermedia.Controllers;
using Sitecore.Hypermedia.Services;
using Sitecore.Workflows;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Controllers
{
    public class WorkboxControllerTest
    {
        [Theory, DefaultAutoData]
        public void GetReturnsEmptyListIfNothingFound(
           IWorkflowService service,
           HttpRequestMessage request)
        {
            var sut = new WorkboxController(service) { Request = request };
            var result = sut.Get();
            Assert.Empty(result);
        }

        [Theory, DefaultAutoData]
        public void GetReturnsWorkflowListIfFound(
          IWorkflowService service,
          HttpRequestMessage request,
          IEnumerable<IWorkflow> workflows)
        {
            service.GetWorkflows().Returns(workflows);
            var sut = new WorkboxController(service) { Request = request };
            var result = sut.Get();
            Assert.True(result.ToList().Count == 3);
        }

        [Theory, DefaultAutoData]
        public void ExecuteWorkflowCommandReturnsBadRequestIfCommandNotAllowed(
          IWorkflowService service,
          HttpRequestMessage request,
          IEnumerable<IWorkflow> workflows,
          Guid itemId,
          string commandId)
        {
            var sut = new WorkboxController(service) { Request = request };
            var result = sut.ExecuteWorkflowCommand(itemId, commandId);
            Assert.IsType<BadRequestResult>(result);
        }

        [Theory, DefaultAutoData]
        public void ExecuteWorkflowCommandExecutesCommandAndReturnsOk(
          IWorkflowService service,
          HttpRequestMessage request,
          IEnumerable<IWorkflow> workflows,
          Guid itemId,
          string commandId)
        {
            service.CanExecuteWorkflowCommand(itemId, commandId)
                .Returns(true);
            var sut = new WorkboxController(service) { Request = request };
            var result = sut.ExecuteWorkflowCommand(itemId, commandId);
            service.Received().ExecuteWorkflowCommand(itemId, commandId);
            Assert.IsType<OkResult>(result);
        }
    }
}