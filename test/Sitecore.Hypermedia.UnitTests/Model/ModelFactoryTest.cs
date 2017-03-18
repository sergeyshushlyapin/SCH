using System;
using System.Collections.Generic;
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
        public void CreateWorkboxModelReturnsName(
            ModelFactory sut,
            IWorkflow workflow,
            string expected)
        {
            workflow.Appearance.DisplayName.Returns(expected);
            var actual = sut.CreateWorkboxModel(workflow);
            Assert.Equal(expected, actual.Name);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkboxModelReturnsWellFormattedHref(
            HttpRequestMessage request,
            ModelFactory sut,
            IWorkflow workflow,
            ID notFormattedWorkflowId,
            string rel)
        {
            workflow.WorkflowID.Returns(notFormattedWorkflowId.ToString());
            var wellFormattedWirkflowId = notFormattedWorkflowId.Guid;
            var expected = $"{request.RequestUri}api/sch/workflows/{wellFormattedWirkflowId}";
            var actual = sut.CreateWorkboxModel(workflow).Url;
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkboxModelReturnsStatesWithItems(
            [Frozen] IWorkflowService service,
            ModelFactory sut,
            IWorkflow workflow,
            string workflowId,
            WorkflowState[] states)
        {
            workflow.WorkflowID.Returns(workflowId);
            service.GetWorkflowStatesWithItems(workflowId)
                .Returns(states);
            var actual = sut.CreateWorkboxModel(workflow);
            Assert.Equal(3, actual.States.Count);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowModelReturnsName(
            ModelFactory sut,
            IWorkflow workflow,
            string expected)
        {
            workflow.Appearance.DisplayName.Returns(expected);
            var actual = sut.Create(workflow);
            Assert.Equal(expected, actual.Name);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowModelReturnsWellFormattedHref(
            HttpRequestMessage request,
            ModelFactory sut,
            IWorkflow workflow,
            ID notFormattedWorkflowId,
            string rel)
        {
            workflow.WorkflowID.Returns(notFormattedWorkflowId.ToString());
            var wellFormattedWirkflowId = notFormattedWorkflowId.Guid;
            var expected = $"{request.RequestUri}api/sch/workflows/{wellFormattedWirkflowId}";
            var actual = sut.Create(workflow).Url;
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowModelReturnsStatesWithItems(
            [Frozen] IWorkflowService service,
            ModelFactory sut,
            IWorkflow workflow,
            string workflowId,
            WorkflowState[] states)
        {
            workflow.WorkflowID.Returns(workflowId);
            service.GetWorkflowStates(workflowId)
                .Returns(states);
            var actual = sut.Create(workflow);
            Assert.Equal(3, actual.States.Count);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkboxStateModelReturnsWellFormattedHref(
            HttpRequestMessage request,
            ModelFactory sut,
            ID notFormattedWorkflowId,
            ID notFormattedStateId,
            string rel,
            string displayName,
            string icon,
            bool finalState)
        {
            var state = new WorkflowState(notFormattedStateId.ToString(), displayName, icon, finalState);
            var expected = $"{request.RequestUri}api/sch/workbox/{notFormattedWorkflowId.Guid}/states/{notFormattedStateId.Guid}";
            var actual = sut.Create(notFormattedWorkflowId.ToString(), state).Links.Single().Href;
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkboxStateModelReturnsStatesWithItems(
            HttpRequestMessage request,
            [Frozen] IWorkflowService service,
            ModelFactory sut,
            string workflowId,
            WorkflowState state,
            DataUri[] items)
        {
            service.GetItemsInState(workflowId, state.StateID)
                .Returns(items);
            var actual = sut.Create(workflowId, state);
            Assert.True(actual.Items.Count() == 3);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowItemModelReturnsItemName(
            HttpRequestMessage request,
            [Frozen] IWorkflowService service,
            ModelFactory sut,
            DataUri dataUri,
            string workflowId,
            string stateId,
            string rel,
            string expected)
        {
            service.GetItemName(dataUri.ItemID).Returns(expected);
            var actual = sut.Create(dataUri, workflowId, stateId).Name;
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowItemModelReturnsWellFormattedHref(
            HttpRequestMessage request,
            ModelFactory sut,
            DataUri dataUri,
            string workflowId,
            string stateId,
            string rel)
        {
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
            HttpRequestMessage request,
            [Frozen(Matching.ImplementedInterfaces)] WorkflowService service,
            ModelFactory sut,
            DataUri dataUri,
            string rel)
        {
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