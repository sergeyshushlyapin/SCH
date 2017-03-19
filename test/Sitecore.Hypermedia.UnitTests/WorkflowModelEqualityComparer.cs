using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class WorkflowModelEqualityComparer : IEqualityComparer<WorkflowModel>
    {
        public bool Equals(WorkflowModel x, WorkflowModel y)
        {
            if (ReferenceEquals(x, y))
                return true;

            return Equals(x.Name, y.Name)
                   && Equals(x.Url, y.Url)
                   && Equals(x.States_Url, y.States_Url);
        }

        public int GetHashCode(WorkflowModel obj)
        {
            return 0;
        }
    }
}