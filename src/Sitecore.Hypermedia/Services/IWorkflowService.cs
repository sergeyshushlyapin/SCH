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

        WorkflowState GetWorkflowState(string workflowId, string stateId);

        IEnumerable<DataUri> GetItemsInState(string workflowId, string stateId);

        string GetItemName(ID id);

        IEnumerable<WorkflowCommand> GetAllowedCommands(string workflowId, string stateId);

        bool CanExecuteWorkflowCommand(Guid itemId, string commandId);

        void ExecuteWorkflowCommand(Guid itemId, string commandId);
    }
}