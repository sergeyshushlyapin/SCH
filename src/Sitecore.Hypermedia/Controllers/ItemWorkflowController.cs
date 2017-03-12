using System;
using System.Web.Http;
using Sitecore.Hypermedia.Services;

namespace Sitecore.Hypermedia.Controllers
{
    public class ItemWorkflowController : ApiController
    {
        private readonly IItemWorkflowService _service;

        public ItemWorkflowController(IItemWorkflowService service)
        {
            _service = service;
        }

        [Route("api/items/{itemId}")]
        public IHttpActionResult GetItem(Guid itemId)
        {
            var item = _service.GetItem(itemId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }
    }
}