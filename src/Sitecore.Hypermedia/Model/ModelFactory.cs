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
        private readonly IWorkboxService _service;
        private readonly UrlHelper _urlHelper;

        public ModelFactory(HttpRequestMessage request, IWorkboxService service)
        {
            _service = service;
            _urlHelper = new UrlHelper(request);
        }

        public WorkflowModel Create(IWorkflow workflow)
        {
            var workflowId = FormatId(workflow.WorkflowID);

            return new WorkflowModel
            {
                Name = workflow.Appearance.DisplayName,
                Links = new List<LinkModel>
                {
                    CreateLink(
                        _urlHelper.Link(
                            "Workflow", new {workflowId}), "self")
                },
                States = new List<WorkflowStateModel>(
                    _service.GetWorkflowStates(workflowId).Select(x => Create(workflow.WorkflowID, x)))
            };
        }

        public WorkflowStateModel Create(string workflowId, WorkflowState workflowState)
        {
            workflowId = FormatId(workflowId);
            var stateId = FormatId(workflowState.StateID);
            var stateLink = _urlHelper.Link("WorkflowState", new { workflowId, stateId });

            var model = new WorkflowStateModel
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

        public WorkflowItemModel Create(DataUri uri, string workflowId, string stateId)
        {
            workflowId = FormatId(workflowId);
            stateId = FormatId(stateId);

            var model = new WorkflowItemModel
            {
                Name = _service.GetItemName(uri.ItemID),
                Language = uri.Language.Name,
                Version = uri.Version.Number,
                Links = new List<LinkModel>
                {
                    CreateLink(_urlHelper.Link("Item", new {itemId = uri.ItemID.Guid}), "self")
                }
            };

            var commands = _service.GetAllowedCommands(stateId);
            foreach (var commandId in commands)
            {
                model.Links.Add(
                    CreateLink(
                        _urlHelper.Link(
                            "WorkflowCommand",
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