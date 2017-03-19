using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class WorkflowStateModelEqualityComparer : IEqualityComparer<WorkflowStateModel>
    {
        public bool Equals(WorkflowStateModel x, WorkflowStateModel y)
        {
            if (ReferenceEquals(x, y))
                return true;

            return Equals(x.Name, y.Name)
                   && Equals(x.Name, y.Name)
                   && Equals(x.FinalState, y.FinalState)
                   && Equals(x.Url, y.Url)
                   && Equals(x.Workflow_Url, y.Workflow_Url);
        }

        public int GetHashCode(WorkflowStateModel obj)
        {
            return 0;
        }
    }
}