﻿using System;
using NSubstitute;
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
    public class ItemWorkflowServiceTest
    {
        [Theory, DefaultAutoData]
        public void GetItemReturnsNullIfNotFound(
            Database database,
            Guid itemId)
        {
            var sut = new ItemWorkflowService(database);
            var actual = sut.GetItem(itemId);
            Assert.Null(actual);
        }

        [Theory, DefaultAutoData]
        public void GetItemReturnsValidItemModelIfFound(
            Database database,
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
            var sut = new ItemWorkflowService(database);
            var expected = new ItemModel
            {
                Id = item.ID.Guid,
                Language = language.Name,
                Version = version.Number,
                Name = name,
                Title = title
            };

            var actual = sut.GetItem(itemId);

            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void GetItemReturnsValidItemModeWithWorkflowlIfFound(
            Database database,
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
            var sut = new ItemWorkflowService(database);
            var expected = new WorkflowModel
            {
                Id = workflow.WorkflowID,
                Name = workflow.Appearance.DisplayName,
                StateId = workflowState.StateID,
                StateName = workflowState.DisplayName,
                FinalState = workflowState.FinalState
            };

            var actual = sut.GetItem(itemId).Workflow;

            Assert.Equal(expected, actual);
        }

        [Theory, DefaultAutoData]
        public void UpdateThrowsIfNoItemFound(
            Database database,
            ItemModel model)
        {
            var sut = new ItemWorkflowService(database);
            var exception = Assert.Throws<InvalidOperationException>(() =>
                sut.Update(model));
            Assert.Equal($"Item {model.Id} not found.", exception.Message);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsFalseIfStateIsNotValid(
           Database database,
           Guid itemId,
           string commandId)
        {
            var sut = new ItemWorkflowService(database);
            var actual = sut.CanExecuteWorkflowCommand(itemId, commandId);
            Assert.False(actual);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsFalseIfNoWorkflowFound(
            Database database,
            Guid itemId,
            string commandId,
            Item item)
        {
            database.GetItem(new ID(itemId)).Returns(item);
            var sut = new ItemWorkflowService(database);
            var actual = sut.CanExecuteWorkflowCommand(itemId, commandId);
            Assert.False(actual);
        }

        [Theory, DefaultAutoData]
        public void CanExecuteWorkflowCommandReturnsTrueIfStateIsValid(
            Database database,
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
            var sut = new ItemWorkflowService(database);

            var actual = sut.CanExecuteWorkflowCommand(
                itemId, command.CommandID);

            Assert.True(actual);
        }
    }
}
