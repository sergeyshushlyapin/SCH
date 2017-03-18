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
                ItemsUrl = linkHelper.Link("SchItems", new { }),
                WorkboxUrl = linkHelper.Link("SchWorkbox", new { }),
            };

            return model;
        }
    }
}