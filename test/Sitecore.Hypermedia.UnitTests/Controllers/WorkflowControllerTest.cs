using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Results;
using NSubstitute;
using Sitecore.Hypermedia.Controllers;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;
using Sitecore.Workflows;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Controllers
{
    public class WorkflowControllerTest
    {
        [Theory, DefaultAutoData]
        public void GetReturnsEmptyListIfNothingFound(
           IWorkflowService service,
           HttpRequestMessage request)
        {
            var sut = new WorkflowController(service) { Request = request };
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
        public void GetByUnknownWorkflowIdReturnsNotFoundResult(
            IWorkflowService service,
            string workflowId)
        {
            service.GetWorkflow(workflowId)
                .ReturnsForAnyArgs(Arg.Any<IWorkflow>());
            var sut = new WorkflowController(service);
            var result = sut.Get(workflowId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory, DefaultAutoData]
        public void GetByKnownWorkflowIdReturnsOkResult(
            IWorkflowService service,
            string workflowId,
            IWorkflow workflow,
            HttpRequestMessage request)
        {
            service.GetWorkflow(workflowId).Returns(workflow);
            var sut = new WorkflowController(service) { Request = request };
            var result = sut.Get(workflowId);
            Assert.IsType<OkNegotiatedContentResult<WorkflowModel>>(result);
        }
    }
}