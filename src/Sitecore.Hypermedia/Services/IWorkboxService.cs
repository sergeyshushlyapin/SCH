using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Workflows;

namespace Sitecore.Hypermedia.Services
{
    public interface IWorkboxService
    {
        IEnumerable<IWorkflow> GetWorkflows();

        IWorkflow GetWorkflow(string workflowId);

        IEnumerable<WorkflowState> GetWorkflowStates(string workflowId);

        WorkflowState GetWorkflowState(string workflowId, string workflowStateId);

        IEnumerable<DataUri> GetItemsInState(string workflowId, string workflowStateId);

        string GetItemName(ID id);
    }
}