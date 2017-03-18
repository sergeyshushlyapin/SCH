using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Sitecore.Hypermedia.Model;
using Sitecore.Hypermedia.Services;

namespace Sitecore.Hypermedia.Controllers
{
    [RoutePrefix("api/sch/workflows")]
    public class WorkflowController : ApiController
    {
        private readonly IWorkflowService _service;
        private ModelFactory _modelFactory;

        public WorkflowController(IWorkflowService service)
        {
            _service = service;
        }

        protected ModelFactory ModelFactory => _modelFactory ?? (_modelFactory = new ModelFactory(Request, _service));

        [Route("", Name = "SchWorkflows")]
        public IEnumerable<WorkflowModel> Get()
        {
            var workflows = _service.GetWorkflows();
            return workflows.Select(ModelFactory.Create);
        }

        [Route("{workflowId}", Name = "SchWorkflow")]
        public IHttpActionResult Get(string workflowId)
        {
            var workflow = _service.GetWorkflow(workflowId);
            if (workflow == null)
                return NotFound();

            return Ok(ModelFactory.Create(workflow));
        }

        [Route("{workflowId}/states")]
        public IHttpActionResult GetStates(string workflowId)
        {
            var workflowStates = _service.GetWorkflowStates(workflowId);
            if (workflowStates == null)
                return NotFound();

            return Ok(workflowStates.Select(x => ModelFactory.Create(workflowId, x)));
        }

        [Route("{workflowId}/states/{stateId}", Name = "SchWorkflowState")]
        public IHttpActionResult GetStates(string workflowId, string stateId)
        {
            var workflowState = _service.GetWorkflowState(workflowId, stateId);
            if (workflowState == null)
                return NotFound();

            return Ok(ModelFactory.Create(workflowId, workflowState));
        }
    }
}