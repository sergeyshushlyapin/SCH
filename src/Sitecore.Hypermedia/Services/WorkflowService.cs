using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Hypermedia.Model;
using Sitecore.SecurityModel;
using Sitecore.Workflows;

namespace Sitecore.Hypermedia.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly Database _database;

        public WorkflowService(Database database)
        {
            _database = database;
        }

        public IEnumerable<IWorkflow> GetWorkflows()
        {
            return _database.WorkflowProvider.GetWorkflows()
                ?? Enumerable.Empty<IWorkflow>();
        }

        public IWorkflow GetWorkflow(string workflowId)
        {
            return _database.WorkflowProvider.GetWorkflow(workflowId);
        }

        public IEnumerable<WorkflowState> GetWorkflowStates(string workflowId)
        {
            var workflow = GetWorkflow(workflowId);
            return workflow?.GetStates()
                ?? Enumerable.Empty<WorkflowState>();
        }

        public IEnumerable<WorkflowState> GetWorkflowStatesWithItems(string workflowId)
        {
            var workflow = GetWorkflow(workflowId);
            return workflow?.GetStates()
                .Where(x => !x.FinalState)
                .Where(x => workflow.GetItems(x.StateID).Any())
                ?? Enumerable.Empty<WorkflowState>();
        }

        public WorkflowState GetWorkflowState(string workflowId, string workflowStateId)
        {
            return GetWorkflow(workflowId)?.GetState(workflowStateId);
        }

        public IEnumerable<DataUri> GetItemsInState(string workflowId, string workflowStateId)
        {
            return GetWorkflow(workflowId)?.GetItems(workflowStateId)
                ?? Enumerable.Empty<DataUri>();
        }

        public string GetItemName(ID id)
        {
            return this._database.GetItem(id)?.Name;
        }

        public IEnumerable<WorkflowCommand> GetAllowedCommands(
            string workflowId,
            string stateId)
        {
            var workflow = GetWorkflow(workflowId);
            if (workflow == null)
                throw new InvalidOperationException($"Workflow '{workflowId}' not found.");

            using (new SecurityDisabler())
            {
                return workflow.GetCommands(stateId)
                       ?? Enumerable.Empty<WorkflowCommand>();
            }
        }

        public void ExecuteWorkflowCommand(Guid itemId, string commandId)
        {
            var item = _database.GetItem(new ID(itemId));
            var workflow = item.State.GetWorkflow();
            workflow.Execute(
                commandId, item, new StringDictionary(), false);
        }

        public bool CanExecuteWorkflowCommand(Guid itemId, string commandId)
        {
            commandId = ModelFactory.FormatId(commandId);

            // TODO: Remove SecurityDisabler
            using (new SecurityDisabler())
            {
                var item = _database.GetItem(new ID(itemId));
                var workflow = item?.State?.GetWorkflow();
                if (workflow == null)
                    return false;

                return workflow.GetCommands(item)
                    .Any(c => ModelFactory.FormatId(c.CommandID).Equals(
                        commandId, StringComparison.OrdinalIgnoreCase));
            }
        }
    }

    public class WorkflowCommandModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}