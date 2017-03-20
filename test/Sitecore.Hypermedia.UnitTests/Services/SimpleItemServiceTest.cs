using System;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;
using Sitecore.Workflows;
using Xunit;
using Version = Sitecore.Data.Version;

namespace Sitecore.Hypermedia.UnitTests.Services
{
    public class SimpleItemServiceTest
    {
        [Theory, DefaultAutoData]
        public void GetItemReturnsNullIfNotFound(
            [Frozen] Database database,
            SimpleItemService sut,
            Guid itemId)
        {
            var actual = sut.GetItem(itemId);
            Assert.Null(actual);
        }

        [Theory, DefaultAutoData]
        public void GetItemReturnsValidItemModelIfFound(
            [Frozen] Database database,
            SimpleItemService sut,
            Guid itemId,
            Item item,
            Language language,
            Version version,
            string name,
            string title)
        {
            item.Language.Returns(language);
            item.Version.Returns(version);
            item.Name.Returns(name);
            item["Title"].Returns(title);
            database.GetItem(new ID(itemId)).Returns(item);
            var expected = new SimpleItemModel
            {
                Id = item.ID.Guid,
                Language = language.Name,
                Version = version.Number,
                Name = name,
                Title = title
            };

            var actual = sut.GetItem(itemId);

            Assert.Equal(expected, actual, new SimpleItemModelEqualityComparer());
        }

        [Theory, DefaultAutoData]
        public void GetItemReturnsValidItemModeWithWorkflowlIfFound(
            [Frozen] Database database,
            SimpleItemService sut,
            Guid itemId,
            Item item,
            Language language,
            Version version,
            ItemState state,
            IWorkflow workflow,
            WorkflowState workflowState)
        {
            item.Language.Returns(language);
            item.Version.Returns(version);
            database.GetItem(new ID(itemId)).Returns(item);
            item.State.Returns(state);
            state.GetWorkflow().Returns(workflow);
            workflow.GetState(item).Returns(workflowState);
            var expected = new SimpleWorkflowModel
            {
                Id = workflow.WorkflowID,
                Name = workflow.Appearance.DisplayName,
                StateId = workflowState.StateID,
                StateName = workflowState.DisplayName,
                FinalState = workflowState.FinalState
            };

            var actual = sut.GetItem(itemId).Workflow;

            Assert.Equal(expected, actual, new SimpleWorkflowModelEqualityComparer());
        }

        [Theory, DefaultAutoData]
        public void UpdateThrowsIfNoItemFound(
            [Frozen] Database database,
            SimpleItemService sut,
            SimpleItemModel model)
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                sut.Update(model));
            Assert.Equal($"Item {model.Id} not found.", exception.Message);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsFalseIfStateIsNotValid(
            [Frozen] Database database,
            SimpleItemService sut,
            Guid itemId,
            string commandId)
        {
            var actual = sut.CanExecuteWorkflowCommand(itemId, commandId);
            Assert.False(actual);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsFalseIfNoWorkflowFound(
            [Frozen] Database database,
            SimpleItemService sut, Guid itemId,
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
            SimpleItemService sut,
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
    }
}