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
        public void CreateWorkflowModelReturnsValidWorkflowModel(
            HttpRequestMessage request,
            ModelFactory sut,
            IWorkflow workflow,
            string name,
                        ID notFormattedWorkflowId)
        {
            workflow.Appearance.DisplayName.Returns(name);
            workflow.WorkflowID.Returns(notFormattedWorkflowId.ToString());
            var wellFormattedWorkflowId = notFormattedWorkflowId.Guid;

            var expected = new WorkflowModel
            {
                Name = name,
                Url = $"{request.RequestUri}api/sch/workflows/{wellFormattedWorkflowId}",
                States_Url = $"{request.RequestUri}api/sch/workflows/{wellFormattedWorkflowId}/states"
            };

            var actual = sut.Create(workflow);

            Assert.Equal(expected, actual, new WorkflowModelEqualityComparer());
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
            var expected = $"{request.RequestUri}api/sch/workflows/{notFormattedWorkflowId.Guid}/states/{notFormattedStateId.Guid}";
            var actual = sut.CreateWorkboxState(notFormattedWorkflowId.ToString(), state).Links.Single().Href;
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
            var actual = sut.CreateWorkboxState(workflowId, state);
            Assert.True(actual.Items.Count() == 3);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowStateModelReturnsValidStateModel(
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
            var wellFormatterWorkflowId = notFormattedWorkflowId.Guid.ToString();
            var expected = new WorkflowStateModel
            {
                Name = displayName,
                FinalState = finalState,
                Url = $"{request.RequestUri}api/sch/workflows/{wellFormatterWorkflowId}/states/{notFormattedStateId.Guid}",
                Workflow_Url = $"{request.RequestUri}api/sch/workflows/{wellFormatterWorkflowId}"
            };

            var actual = sut.Create(notFormattedWorkflowId.ToString(), state);

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Workflow_Url, actual.Workflow_Url);
            Assert.Equal(expected, actual, new WorkflowStateModelEqualityComparer());
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
            var actual = sut.Create(dataUri, workflowId, stateId).Url;
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CreateWorkflowItemModelReturnsSubmitItemLink(
            HttpRequestMessage request,
            [Frozen] IWorkflowService service,
            ModelFactory sut,
            string workflowId,
            string stateId,
            DataUri dataUri,
            string itemName,
            ID commandId,
            string commandName,
            string icon,
            bool hasUi,
            string rel)
        {
            service.GetItemName(dataUri.ItemID).Returns(itemName);
            service.GetAllowedCommands(workflowId, stateId)
                .Returns(new[]
                {
                    new WorkflowCommand(commandId.ToString(), commandName, icon, hasUi),
                });

            var itemId = dataUri.ItemID.Guid;
            var expectedCommandId = commandId.Guid;
            var expected = new WorkboxItemModel
            {
                Name = itemName,
                Url = $"{request.RequestUri}api/sch/items/{itemId}",
                Commands = new List<LinkModel> { new LinkModel
                {
                    Href = $"{request.RequestUri}api/sch/workbox/{itemId}/{expectedCommandId}",
                    Rel = commandName,
                    Method = "POST"
                }}
            };

            var actual = sut.Create(dataUri, workflowId, stateId);

            Assert.Equal(expected, actual, new WorkboxItemModelEqualityComparer());
        }
    }
}