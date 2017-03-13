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
                    workflow.GetStates().Select(x => Create(workflow.WorkflowID, x)))
            };
        }

        public WorkflowStateModel Create(string workflowId, WorkflowState workflowState)
        {
            workflowId = FormatId(workflowId);
            var workflowStateId = FormatId(workflowState.StateID);

            return new WorkflowStateModel
            {
                Name = workflowState.DisplayName,
                FinalState = workflowState.FinalState,
                Items = new List<WorkflowItemModel>(
                    _service.GetItemsInState(workflowId, workflowState.StateID)
                    .Select(Create)),
                Links = new List<LinkModel>
                {
                    CreateLink(
                        _urlHelper.Link(
                            "WorkflowState",
                            new {workflowId, workflowStateId }),
                        "self")
                }
            };
        }

        public WorkflowItemModel Create(DataUri uri)
        {
            return new WorkflowItemModel
            {
                Name = uri.Path,
                Language = uri.Language.Name,
                Version = uri.Version.Number,
                Links = new List<LinkModel>
                {
                    CreateLink(_urlHelper.Link("Item", new {itemId = uri.ItemID.Guid}), "self")
                }
            };
        }

        public LinkModel CreateLink(string url, string rel, string method = "GET")
        {
            return new LinkModel { Href = url, Rel = rel, Method = method };
        }

        private static string FormatId(string workflowId)
        {
            if (ID.IsID(workflowId))
            {
                workflowId = ID.Parse(workflowId).Guid.ToString();
            }
            return workflowId;
        }
    }
}