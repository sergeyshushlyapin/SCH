using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Workflows;

namespace Sitecore.Hypermedia.Services
{
    public class WorkboxService : IWorkboxService
    {
        private readonly Database _database;

        public WorkboxService(Database database)
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
    }
}