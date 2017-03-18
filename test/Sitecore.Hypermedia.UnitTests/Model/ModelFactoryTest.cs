using System.Linq;
using System.Net.Http;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using Sitecore.Data;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;
using Sitecore.Workflows;
using Xunit;
using static Sitecore.Hypermedia.Model.ModelFactory;

namespace Sitecore.Hypermedia.UnitTests.Model
{
    public class ModelFactoryTest
    {
        [Theory, DefaultAutoData]
        public void CreateWorkflowModelReturnsWellFormattedHref(
            [Frozen] HttpRequestMessage request,
            ModelFactory sut,
            IWorkflow workflow,
            ID notFormattedWorkflowId,
            string rel)
        {
            request.SetConfiguration(new DefaultHttpConfiguration());
            workflow.WorkflowID.Returns(notFormattedWorkflowId.ToString());
            var expected = $"{request.RequestUri}api/sch/workbox/{notFormattedWorkflowId.Guid}";
            var actual = sut.Create(workflow).Links.Single().Href;
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowStateModelReturnsWellFormattedHref(
            [Frozen] HttpRequestMessage request,
            ModelFactory sut,
            ID notFormattedWorkflowId,
            ID notFormattedStateId,
            string rel,
            string displayName,
            string icon,
            bool finalState)
        {
            request.SetConfiguration(new DefaultHttpConfiguration());
            var state = new WorkflowState(notFormattedStateId.ToString(), displayName, icon, finalState);
            var expected = $"{request.RequestUri}api/sch/workbox/{notFormattedWorkflowId.Guid}/states/{notFormattedStateId.Guid}";
            var actual = sut.Create(notFormattedWorkflowId.ToString(), state).Links.Single().Href;
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowItemModelReturnsItemName(
            [Frozen] HttpRequestMessage request,
            [Frozen] IWorkboxService service,
            ModelFactory sut,
            DataUri dataUri,
            string workflowId,
            string stateId,
            string rel,
            string expected)
        {
            request.SetConfiguration(new DefaultHttpConfiguration());
            service.GetItemName(dataUri.ItemID).Returns(expected);
            var actual = sut.Create(dataUri, workflowId, stateId).Name;
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowItemModelReturnsWellFormattedHref(
            [Frozen] HttpRequestMessage request,
            ModelFactory sut,
            DataUri dataUri,
            string workflowId,
            string stateId,
            string rel)
        {
            request.SetConfiguration(new DefaultHttpConfiguration());
            var expected = $"{request.RequestUri}api/sch/items/{dataUri.ItemID.Guid}";
            var actual = sut.Create(dataUri, workflowId, stateId).Links.Single().Href;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineDefaultAutoData(SampleWorkflow.DraftStateId)]
        //[InlineDefaultAutoData(SampleWorkflow.AwaitingApprovalStateId)]
        public void CreateWorkflowItemModelReturnsSubmitItemLink(
            string stateId,
            string workflowId,
            [Frozen] HttpRequestMessage request,
            [Frozen(Matching.ImplementedInterfaces)] WorkboxService service,
            ModelFactory sut,
            DataUri dataUri,
            string rel)
        {
            request.SetConfiguration(new DefaultHttpConfiguration());
            var commandId = service.GetAllowedCommands(stateId).First();
            var expected = new LinkModel
            {
                Href = $"{request.RequestUri}api/sch/workbox/{FormatId(workflowId)}/states/{FormatId(stateId)}/commands/{commandId}",
                Rel = "execute",
                Method = "POST"
            };

            var actual = sut.Create(dataUri, workflowId, stateId);

            Assert.Contains(expected, actual.Links, new LinkModelEqualityComparer());
        }
    }
}