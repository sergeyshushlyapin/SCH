using System.Linq;
using System.Web.Http;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;
using System.Collections.Generic;

namespace Sitecore.Hypermedia.Controllers
{
    public class WorkboxController : ApiController
    {
        private readonly IWorkboxService _service;
        private ModelFactory _modelFactory;

        public WorkboxController(IWorkboxService service)
        {
            _service = service;
        }

        protected ModelFactory ModelFactory => _modelFactory ?? (_modelFactory = new ModelFactory(Request, _service));

        [Route("api/wb")]
        public IEnumerable<WorkflowModel> Get()
        {
            var workflows = _service.GetWorkflows();
            return workflows.Select(ModelFactory.Create);
        }

        [Route("api/wb/{workflowId}", Name = "Workflow")]
        public IHttpActionResult Get(string workflowId)
        {
            var workflow = _service.GetWorkflow(workflowId);
            if (workflow == null)
                return NotFound();

            return Ok(ModelFactory.Create(workflow));
        }

        [Route("api/wb/{workflowId}/states")]
        public IHttpActionResult GetStates(string workflowId)
        {
            var workflowStates = _service.GetWorkflowStates(workflowId);
            if (workflowStates == null)
                return NotFound();

            return Ok(workflowStates.Select(x => ModelFactory.Create(workflowId, x)));
        }

        [Route("api/wb/{workflowId}/states/{stateId}", Name = "WorkflowState")]
        public IHttpActionResult GetStates(string workflowId, string stateId)
        {
            var workflowState = _service.GetWorkflowState(workflowId, stateId);
            if (workflowState == null)
                return NotFound();

            return Ok(ModelFactory.Create(workflowId, workflowState));
        }

        [HttpPost]
        [Route("api/wb/{workflowId}/states/{stateId}/commands/{commandId}", Name = "WorkflowCommand")]
        public IHttpActionResult GetStates(string workflowId, string stateId, string commandId)
        {
            var workflowState = _service.GetWorkflowState(workflowId, stateId);
            if (workflowState == null)
                return NotFound();

            return Ok(ModelFactory.Create(workflowId, workflowState));
        }
    }
}