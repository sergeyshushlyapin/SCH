using System;
using System.Collections.Generic;
using System.Web.Http;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;

namespace Sitecore.Hypermedia.Controllers
{
    [RoutePrefix("api/sch/items")]
    public class ItemWorkflowController : ApiController
    {
        private readonly IItemWorkflowService _service;

        public ItemWorkflowController(IItemWorkflowService service)
        {
            _service = service;
        }

        [Route("", Name = "SchItems")]
        public IEnumerable<ItemModel> Get()
        {
            return _service.GetContentItems();
        }

        [Route("{itemId}", Name = "SchItem")]
        public ItemModel GetItem(Guid itemId)
        {
            return _service.GetItem(itemId);
        }

        [HttpPatch]
        [Route("{itemId}")]
        public IHttpActionResult UpdateTitle(ItemModel model)
        {
            var item = _service.GetItem(model.Id);
            if (item == null)
                return NotFound();

            _service.Update(model);

            return Ok();
        }

        [HttpPost]
        [Route("{itemId}/workflow/{commandId}")]
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