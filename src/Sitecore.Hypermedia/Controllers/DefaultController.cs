using System.Web.Http;
using System.Web.Http.Routing;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.Controllers
{
    public class DefaultController : ApiController
    {
        [Route("api/sch")]
        public DefaultUrls Get()
        {
            var linkHelper = new UrlHelper(Request);
            var model = new DefaultUrls()
            {
                Items_Url = linkHelper.Link("SchItems", new { }),
                Workbox_Url = linkHelper.Link("SchWorkbox", new { }),
                Workflows_Url = linkHelper.Link("SchWorkflows", new { })
            };

            return model;
        }
    }
}