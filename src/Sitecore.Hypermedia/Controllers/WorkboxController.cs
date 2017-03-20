using System;
using System.Linq;
using System.Web.Http;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;
using System.Collections.Generic;

namespace Sitecore.Hypermedia.Controllers
{
    [RoutePrefix("api/sch/workbox")]
    public class WorkboxController : ApiController
    {
        private readonly IWorkflowService _service;
        private ModelFactory _modelFactory;

        public WorkboxController(IWorkflowService service)
        {
            _service = service;
        }

        protected ModelFactory ModelFactory => _modelFactory ?? (_modelFactory = new ModelFactory(Request, _service));

        [Route("", Name = "SchWorkbox")]
        public IEnumerable<WorkboxModel> Get()
        {
            var workflows = _service.GetWorkflows();
            return workflows.Select(ModelFactory.CreateWorkboxModel);
        }

        [HttpPost]
        [Route("{itemId}/{commandId}", Name = "SchWorkboxCommand")]
        public IHttpActionResult ExecuteWorkflowCommand(
            Guid itemId, string commandId)
        {
            if (!_service.CanExecuteWorkflowCommand(itemId, commandId))
                return BadRequest();

            _service.ExecuteWorkflowCommand(itemId, commandId);

            return Ok();
        }
    }
}