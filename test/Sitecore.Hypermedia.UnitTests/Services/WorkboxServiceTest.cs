using System.Linq;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using Sitecore.Data;
using Sitecore.Hypermedia.Services;
using Sitecore.Workflows;
using Xunit;

namespace Sitecore.Hypermedia.UnitTests.Services
{
    public class WorkboxServiceTest
    {
        [Theory, DefaultAutoData]
        public void GetWorkflowsReturnsEmplyListIfNoWorkflowsFound(
            [Frozen] Database database,
            WorkboxService sut)
        {
            database.WorkflowProvider.GetWorkflows()
                .Returns(Arg.Any<IWorkflow[]>());
            var actual = sut.GetWorkflows();
            Assert.Empty(actual);
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowsReturnsWorkflows(
            [Frozen] Database database,
            WorkboxService sut,
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
            WorkboxService sut,
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
            WorkboxService sut,
            string workflowId)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .ReturnsForAnyArgs(Arg.Any<IWorkflow>());
            var actual = sut.GetWorkflowStates(workflowId);
            Assert.Empty(actual);
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowStatesReturnsNonFinalStates(
            [Frozen] Database database,
            WorkboxService sut,
            string workflowId,
            IWorkflow workflow,
            string stateId1,
            string stateId2,
            string displayName,
            string icon)
        {
            database.WorkflowProvider.GetWorkflow(workflowId)
                .Returns(workflow);
            var finalState = new WorkflowState(stateId1, displayName, icon, true);
            var notFinalState = new WorkflowState(stateId2, displayName, icon, false);
            workflow.GetStates()
                .Returns(new[] { finalState, notFinalState });

            var actual = sut.GetWorkflowStates(workflowId);

            Assert.Equal(notFinalState, actual.Single());
        }

        [Theory, DefaultAutoData]
        public void GetWorkflowStateReturnsNullIfNoWorkflowFound(
            [Frozen] Database database,
            WorkboxService sut,
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
            WorkboxService sut,
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
            WorkboxService sut,
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
            WorkboxService sut,
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
    }
}