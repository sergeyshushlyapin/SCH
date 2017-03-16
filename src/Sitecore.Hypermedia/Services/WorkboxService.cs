using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Collections;
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

        public IEnumerable<Guid> GetAllowedCommands(string workflowStateId)
        {
            ID stateId;
            if (!ID.TryParse(workflowStateId, out stateId))
                return Enumerable.Empty<Guid>();

            const string draftStateId = "{190B1C84-F1BE-47ED-AA41-F42193D9C8FC}";
            const string awaitingApprovalStateId = "{46DA5376-10DC-4B66-B464-AFDAA29DE84F}";
            var submitCommandId = new Guid("{CF6A557D-0B86-4432-BF47-302A18238E74}");
            var approveCommandId = new Guid("{F744CC9C-4BB1-4B38-8D5C-1E9CE7F45D2D}");
            var rejectCommandId = new Guid("{E44F2D64-1EED-42FF-A7DA-C07B834096AC}");

            switch (stateId.ToString())
            {
                case draftStateId:
                    return new[] { submitCommandId };

                case awaitingApprovalStateId:
                    return new[] { approveCommandId, rejectCommandId };

                default:
                    return Enumerable.Empty<Guid>();
            }
        }

        public void ExecuteWorkflowCommand(Guid itemId, string commandId)
        {
            var item = _database.GetItem(new ID(itemId));
            var workflow = item.State.GetWorkflow();
            workflow.Execute(
                commandId, item, new StringDictionary(), false);
        }
    }
}