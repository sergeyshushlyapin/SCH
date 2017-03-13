using System;
using System.Web.Http;
using Sitecore.Hypermedia.Model;
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

        [Route("api/items")]
        public IHttpActionResult Get()
        {
            var item = _service.GetContentItems();
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [Route("api/items/{itemId}")]
        public IHttpActionResult GetItem(Guid itemId)
        {
            var item = _service.GetItem(itemId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPatch]
        [Route("api/items/{itemId}")]
        public IHttpActionResult UpdateTitle(ItemModel model)
        {
            var item = _service.GetItem(model.Id);
            if (item == null)
                return NotFound();

            _service.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("api/items/{itemId}/workflow/{commandId}")]
        public IHttpActionResult ExecuteWorkflowCommand(
            Guid itemId, string commandId)
        {
            var existingItem = _service.GetItem(itemId);
            if (existingItem == null)
                return NotFound();

            if (!_service.CanExecuteWorkflowCommand(itemId, commandId))
                return BadRequest($"Invalid workflow command: {commandId}.");

            _service.ExecuteWorkflowCommand(itemId, commandId);

            return Ok();
        }
    }
}