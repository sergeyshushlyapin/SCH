using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
           IWorkboxService service,
           HttpRequestMessage request)
        {
            var sut = new WorkboxController(service) { Request = request };
            var result = sut.Get();
            Assert.Empty(result);
        }

        [Theory, DefaultAutoData]
        public void GetReturnsWorkflowListIfFound(
          IWorkboxService service,
          HttpRequestMessage request,
          IEnumerable<IWorkflow> workflows)
        {
            service.GetWorkflows().Returns(workflows);
            request.SetConfiguration(new DefaultHttpConfiguration());
            var sut = new WorkboxController(service) { Request = request };

            var result = sut.Get();

            Assert.True(result.ToList().Count == 3);
        }
    }
}