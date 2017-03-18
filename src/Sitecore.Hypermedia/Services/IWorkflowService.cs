using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Workflows;

namespace Sitecore.Hypermedia.Services
{
    public interface IWorkflowService
    {
        IEnumerable<IWorkflow> GetWorkflows();

        IWorkflow GetWorkflow(string workflowId);

        IEnumerable<WorkflowState> GetWorkflowStates(string workflowId);

        IEnumerable<WorkflowState> GetWorkflowStatesWithItems(string workflowId);

        WorkflowState GetWorkflowState(string workflowId, string workflowStateId);

        IEnumerable<DataUri> GetItemsInState(string workflowId, string workflowStateId);

        string GetItemName(ID id);

        IEnumerable<Guid> GetAllowedCommands(string workflowStateId);

        void ExecuteWorkflowCommand(Guid itemId, string commandId);
    }
}