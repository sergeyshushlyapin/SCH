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
                    {"SchWorkflows", new HttpRoute("api/sch/workflows")},
                    {"SchWorkflow", new HttpRoute("api/sch/workflows/{workflowId}")},
                    {"SchWorkflowState", new HttpRoute("api/sch/workbox/{workflowId}/states/{stateId}")},
                    {"SchWorkflowCommand", new HttpRoute("api/sch/workbox/{workflowId}/states/{stateId}/commands/{commandId}")}
                })
        {
        }
    }
}