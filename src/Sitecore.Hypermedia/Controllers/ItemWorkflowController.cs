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