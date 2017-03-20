using System.Web.Http;
using System.Web.Http.Routing;

namespace Sitecore.Hypermedia.UnitTests
{
    internal class DefaultHttpConfiguration : HttpConfiguration
    {
        public DefaultHttpConfiguration()
            : base(
                new HttpRouteCollection
                {
                    {"SchItems", new HttpRoute("api/sch/items")},
                    {"SchItem", new HttpRoute("api/sch/items/{itemId}")},
                    {"SchWorkbox", new HttpRoute("api/sch/workbox")},
                    {"SchWorkboxCommand", new HttpRoute("api/sch/workbox/{itemId}/{commandId}")},
                    {"SchWorkflows", new HttpRoute("api/sch/workflows")},
                    {"SchWorkflow", new HttpRoute("api/sch/workflows/{workflowId}")},
                    {"SchWorkflowStates", new HttpRoute("api/sch/workflows/{workflowId}/states")},
                    {"SchWorkflowState", new HttpRoute("api/sch/workflows/{workflowId}/states/{stateId}")}
                })
        {
        }
    }
}