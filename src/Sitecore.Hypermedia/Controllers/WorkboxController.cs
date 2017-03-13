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

        protected ModelFactory ModelFactory => _modelFactory ?? (_modelFactory = new ModelFactory(Request));

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

        [Route("api/wb/{workflowId}/states/{workflowStateId}", Name = "WorkflowState")]
        public IHttpActionResult GetStates(string workflowId, string workflowStateId)
        {
            var workflowState = _service.GetWorkflowState(workflowId, workflowStateId);
            if (workflowState == null)
                return NotFound();

            return Ok(ModelFactory.Create(workflowId, workflowState));
        }
    }
}