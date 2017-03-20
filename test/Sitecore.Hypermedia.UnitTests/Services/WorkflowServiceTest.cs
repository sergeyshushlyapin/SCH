using System;
using System.Linq;
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
        public void GetAllowedCommandsThrowsIfNoWorkflowFound(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            string stateId)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(Arg.Any<IWorkflow>());
            var ex = Assert.Throws<InvalidOperationException>(() =>
                sut.GetAllowedCommands(workflowId, stateId));
            Assert.Equal($"Workflow '{workflowId}' not found.", ex.Message);
        }

        [Theory, DefaultAutoData]
        public void GetAllowedCommandsReturnsEmptyListIfNoCommandsFound(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            string stateId,
            IWorkflow workflow)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(workflow);
            var actual = sut.GetAllowedCommands(workflowId, stateId);
            Assert.Empty(actual);
        }

        [Theory, DefaultAutoData]
        public void GetAllowedCommandsReturnsCommandsIfFound(
            [Frozen] Database database,
            WorkflowService sut,
            string workflowId,
            string stateId,
            IWorkflow workflow,
            WorkflowCommand[] expected)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(workflow);
            workflow.GetCommands(stateId).Returns(expected);
            var actual = sut.GetAllowedCommands(workflowId, stateId);
            Assert.Same(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsFalseIfStateIsNotValid(
            [Frozen] Database database,
            WorkflowService sut,
            Guid itemId,
            string commandId)
        {
            var actual = sut.CanExecuteWorkflowCommand(itemId, commandId);
            Assert.False(actual);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsFalseIfNoWorkflowFound(
            [Frozen] Database database,
            WorkflowService sut,
            Guid itemId,
            string commandId,
            Item item)
        {
            database.GetItem(new ID(itemId)).Returns(item);
            var actual = sut.CanExecuteWorkflowCommand(itemId, commandId);
            Assert.False(actual);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsTrueIfStateIsValid(
            [Frozen] Database database,
            WorkflowService sut,
            Guid itemId,
            Item item,
            ItemState state,
            IWorkflow workflow,
            WorkflowCommand command)
        {
            database.GetItem(new ID(itemId)).Returns(item);
            item.State.Returns(state);
            state.GetWorkflow().Returns(workflow);
            workflow.GetCommands(item).Returns(new[] { command });

            var actual = sut.CanExecuteWorkflowCommand(
                itemId, command.CommandID);

            Assert.True(actual);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsTrueIfStateIsValidAndCommandIdIsFormatted(
            [Frozen] Database database,
            WorkflowService sut,
            Guid itemId,
            Item item,
            ItemState state,
            IWorkflow workflow,
            WorkflowCommand command,
            ID notFormattedCommandId,
            string name)
        {
            database.GetItem(new ID(itemId)).Returns(item);
            item.State.Returns(state);
            state.GetWorkflow().Returns(workflow);
            workflow.GetCommands(item).Returns(new[]
            {
                new WorkflowCommand(notFormattedCommandId.ToString(), name, "icon", false),
            });
            var formattedCommandId = notFormattedCommandId.Guid;

            var actual = sut.CanExecuteWorkflowCommand(
                itemId, formattedCommandId.ToString());

            Assert.True(actual);
        }
    }
}