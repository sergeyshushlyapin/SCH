using System.Collections.Generic;
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
            return _database.WorkflowProvider.GetWorkflows();
        }

        public IWorkflow GetWorkflow(string workflowId)
        {
            return _database.WorkflowProvider.GetWorkflow(workflowId);
        }

        public IEnumerable<WorkflowState> GetWorkflowStates(string workflowId)
        {
            return GetWorkflow(workflowId)?.GetStates();
        }

        public WorkflowState GetWorkflowState(string workflowId, string workflowStateId)
        {
            return GetWorkflow(workflowId)?.GetState(workflowStateId);
        }
    }
}