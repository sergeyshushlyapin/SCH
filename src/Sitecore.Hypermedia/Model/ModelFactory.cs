using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using Sitecore.Data;
using Sitecore.Hypermedia.Services;
using Sitecore.Workflows;

namespace Sitecore.Hypermedia.Model
{
    public class ModelFactory
    {
        private readonly IWorkflowService _service;
        private readonly UrlHelper _urlHelper;

        public ModelFactory(HttpRequestMessage request, IWorkflowService service)
        {
            _service = service;
            _urlHelper = new UrlHelper(request);
        }

        public WorkboxModel CreateWorkboxModel(IWorkflow workflow)
        {
            var workflowId = FormatId(workflow.WorkflowID);

            return new WorkboxModel
            {
                Name = workflow.Appearance.DisplayName,
                Url = _urlHelper.Link("SchWorkflow", new { workflowId }),
                States = new List<WorkboxStateModel>(
                    _service.GetWorkflowStatesWithItems(workflowId)
                    .Select(x => CreateWorkboxState(workflowId, x)))
            };
        }

        public WorkflowModel Create(IWorkflow workflow)
        {
            var workflowId = FormatId(workflow.WorkflowID);

            return new WorkflowModel
            {
                Name = workflow.Appearance.DisplayName,
                Url = _urlHelper.Link("SchWorkflow", new { workflowId }),
                States_Url = _urlHelper.Link("SchWorkflowStates", new { workflowId })
            };
        }

        public WorkboxStateModel CreateWorkboxState(string workflowId, WorkflowState workflowState)
        {
            workflowId = FormatId(workflowId);
            var stateId = FormatId(workflowState.StateID);
            var stateLink = _urlHelper.Link("SchWorkflowState", new { workflowId, stateId });

            var model = new WorkboxStateModel
            {
                Name = workflowState.DisplayName,
                FinalState = workflowState.FinalState,
                Items = new List<WorkflowItemModel>(
                    _service.GetItemsInState(workflowId, workflowState.StateID)
                        .Select(x => Create(x, workflowId, stateId))),
                Links = new List<LinkModel>
                {
                    CreateLink(stateLink, "self")
                }
            };

            return model;
        }

        public WorkflowStateModel Create(
            string workflowId,
            WorkflowState state)
        {
            workflowId = FormatId(workflowId);
            var stateId = FormatId(state.StateID);
            var model = new WorkflowStateModel
            {
                Name = state.DisplayName,
                FinalState = state.FinalState,
                Url = _urlHelper.Link("SchWorkflowState", new { workflowId, stateId }),
                Workflow_Url = _urlHelper.Link("SchWorkflow", new { workflowId })
            };

            return model;
        }

        public WorkflowItemModel Create(DataUri uri, string workflowId, string stateId)
        {
            workflowId = FormatId(workflowId);
            stateId = FormatId(stateId);

            var model = new WorkflowItemModel
            {
                Name = _service.GetItemName(uri.ItemID),
                Url = _urlHelper.Link(
                    "SchItem", new { itemId = uri.ItemID.Guid }),
                Commands = new List<LinkModel>()
            };

            var commands = _service.GetAllowedCommands(stateId);
            foreach (var commandId in commands)
            {
                model.Commands.Add(
                    CreateLink(
                        _urlHelper.Link(
                            "SchWorkflowCommand",
                            new { workflowId, stateId, commandId }),
                        // TODO: Add command name
                        "execute",
                        "POST"));
            }

            return model;
        }

        public LinkModel CreateLink(string url, string rel, string method = "GET")
        {
            return new LinkModel { Href = url, Rel = rel, Method = method };
        }

        public static string FormatId(string workflowId)
        {
            if (ID.IsID(workflowId))
            {
                workflowId = ID.Parse(workflowId).Guid.ToString();
            }
            return workflowId;
        }
    }
}