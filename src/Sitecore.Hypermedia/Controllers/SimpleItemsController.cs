using System;
using System.Collections.Generic;
using System.Web.Http;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;

namespace Sitecore.Hypermedia.Controllers
{
    [RoutePrefix("api/items")]
    public class SimpleItemsController : ApiController
    {
        private readonly ISimpleItemService _service;

        public SimpleItemsController(ISimpleItemService service)
        {
            _service = service;
        }

        [Route("")]
        public IEnumerable<SimpleItemModel> Get()
        {
            return _service.GetContentItems();
        }

        [Route("{itemId}")]
        public SimpleItemModel GetItem(Guid itemId)
        {
            return _service.GetItem(itemId);
        }

        [HttpPatch]
        [Route("{itemId}")]
        public IHttpActionResult UpdateTitle(SimpleItemModel model)
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