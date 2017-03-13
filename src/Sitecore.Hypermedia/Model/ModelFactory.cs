using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using Sitecore.Workflows;

namespace Sitecore.Hypermedia.Model
{
    public class ModelFactory
    {
        private readonly UrlHelper _urlHelper;

        public ModelFactory(HttpRequestMessage request)
        {
            _urlHelper = new UrlHelper(request);
        }

        public WorkflowModel Create(IWorkflow workflow)
        {
            return new WorkflowModel
            {
                Name = workflow.Appearance.DisplayName,
                Links = new List<LinkModel>
                {
                    CreateLink(
                        _urlHelper.Link(
                            "Workflow", new {workflowId = workflow.WorkflowID}), "self")
                },
                States = new List<WorkflowStateModel>(
                    workflow.GetStates().Select(x => Create(workflow.WorkflowID, x)))
            };
        }

        public WorkflowStateModel Create(string workflowId, WorkflowState workflowState)
        {
            return new WorkflowStateModel
            {
                Name = workflowState.DisplayName,
                FinalState = workflowState.FinalState,
                Links = new List<LinkModel>
                {
                    CreateLink(
                        _urlHelper.Link(
                            "WorkflowState",
                            new {workflowId, workflowStateId = workflowState.StateID}),
                        "self")
                }
            };
        }

        public LinkModel CreateLink(string url, string rel, string method = "GET")
        {
            return new LinkModel { Href = url, Rel = rel, Method = method };
        }
    }
}