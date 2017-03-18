﻿using System.Linq;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Hypermedia.Services;
using Sitecore.Workflows;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Services
{
    public class WorkflowServiceTest
    {
        [Theory, DefaultAutoData]
        public void GetWorkflowsReturnsEmplyListIfNoWorkflowsFound(
            [Frozen] Database database,
            WorkflowService sut)
        {
            database.WorkflowProvider.GetWorkflows()
                .Returns(Arg.Any<IWorkflow[]>());
            var actual = sut.GetWorkflows();
            Assert.Empty(actual);
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowsReturnsWorkflows(
            [Frozen] Database database,
            WorkflowService sut,
            IWorkflow[] expected)
        {
            database.WorkflowProvider.GetWorkflows()
                .Returns(expected);
            var actual = sut.GetWorkflows();

            Assert.True(expected.SequenceEqual(actual));
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowReturnsWorkflow(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            IWorkflow expected)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(expected);
            var actual = sut.GetWorkflow(workflowId);

            Assert.Same(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowStatesReturnsEmplyListIfNoWorkflowFound(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .ReturnsForAnyArgs(Arg.Any<IWorkflow>());
            var actual = sut.GetWorkflowStates(workflowId);
            Assert.Empty(actual);
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowStatesReturnsAllStates(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            IWorkflow workflow,
            WorkflowState[] expected)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(workflow);
            workflow.GetStates().Returns(expected);

            var actual = sut.GetWorkflowStates(workflowId);

            Assert.Same(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowStatesWithItemsReturnsEmplyListIfNoWorkflowFound(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .ReturnsForAnyArgs(Arg.Any<IWorkflow>());
            var actual = sut.GetWorkflowStatesWithItems(workflowId);
            Assert.Empty(actual);
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowStatesWithItemsReturnsNonFinalStatesWithItemsOnly(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            IWorkflow workflow,
            string stateId1,
            string stateId2,
            string stateId3,
            string displayName,
            string icon,
            DataUri[] itemsInState)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(workflow);
            var finalState = new WorkflowState(stateId1, displayName, icon, true);
            var nonFinalStateWithItems = new WorkflowState(stateId2, displayName, icon, false);
            var nonFinalStateWithoutItems = new WorkflowState(stateId3, displayName, icon, false);
            workflow.GetStates()
                .Returns(new[] { finalState, nonFinalStateWithItems, nonFinalStateWithoutItems });
            workflow.GetItems(nonFinalStateWithItems.StateID).Returns(itemsInState);

            var actual = sut.GetWorkflowStatesWithItems(workflowId);

            Assert.True(new[] { nonFinalStateWithItems }.SequenceEqual(actual));
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowStateReturnsNullIfNoWorkflowFound(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            string workflowStateId)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .ReturnsForAnyArgs(Arg.Any<IWorkflow>());
            var actual = sut.GetWorkflowState(workflowId, workflowStateId);
            Assert.Null(actual);
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowStateReturnsWorkflow(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            string workflowStateId,
            IWorkflow workflow,
            WorkflowState expected)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(workflow);
            workflow.GetState(workflowStateId).Returns(expected);
            var actual = sut.GetWorkflowState(workflowId, workflowStateId);
            Assert.Same(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void GetItemsInStateReturnsEmplyListIfNoWorkflowFound(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            string workflowStateId)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .ReturnsForAnyArgs(Arg.Any<IWorkflow>());
            var actual = sut.GetItemsInState(workflowId, workflowStateId);
            Assert.Empty(actual);
        }

        [Theory, DefaultAutoData]
        public void GetItemsInStateReturnsDataUris(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            string workflowStateId,
            IWorkflow workflow,
            DataUri[] expected)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(workflow);
            workflow.GetItems(workflowStateId).Returns(expected);
            var actual = sut.GetItemsInState(workflowId, workflowStateId);
            Assert.True(expected.SequenceEqual(actual));
        }

        [Theory, DefaultAutoData]
        public void GetItemNameReturnsNullIfNoItemFound(WorkflowService sut, ID id)
        {
            var actual = sut.GetItemName(id);
            Assert.Null(actual);
        }

        [Theory, DefaultAutoData]
        public void GetItemNameReturnsItemNameIfExists(
            [Frozen] Database database,
            WorkflowService sut,
            ID id,
            Item item,
            string expected)
        {
            database.GetItem(id).Returns(item);
            item.Name.Returns(expected);
            var actual = sut.GetItemName(id);
            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void GetAllowedCommandsReturnsEmptyListIfStateIsNotGuid(
            WorkflowService sut,
            string workflowId,
            string workflowStateId)
        {
            var actual = sut.GetAllowedCommands(workflowStateId);
            Assert.Empty(actual);
        }

        [Theory]
        [InlineDefaultAutoData(SampleWorkflow.DraftStateId, "cf6a557d-0b86-4432-bf47-302a18238e74")]
        [InlineDefaultAutoData(SampleWorkflow.AwaitingApprovalStateId, "f744cc9c-4bb1-4b38-8d5c-1e9ce7f45d2d|e44f2d64-1eed-42ff-a7da-c07b834096ac")]
        [InlineDefaultAutoData("{11111111-1111-1111-1111-111111111111}", "")]
        public void GetAllowedCommandsReturnsCommandIds(
            string workflowStateId,
            string expected,
            WorkflowService sut,
            string workflowId)
        {
            var actual = sut.GetAllowedCommands(workflowStateId);
            Assert.Equal(expected, string.Join("|", actual));
        }
    }
}