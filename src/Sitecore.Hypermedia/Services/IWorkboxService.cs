﻿using System.Collections.Generic;
using Sitecore.Workflows;

namespace Sitecore.Hypermedia.Services
{
    public interface IWorkboxService
    {
        IEnumerable<IWorkflow> GetWorkflows();

        IWorkflow GetWorkflow(string workflowId);

        IEnumerable<WorkflowState> GetWorkflowStates(string workflowId);

        WorkflowState GetWorkflowState(string workflowId, string workflowStateId);
    }
}