using System.Collections.Generic;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.UnitTests
{
    public class SimpleWorkflowModelEqualityComparer : IEqualityComparer<SimpleWorkflowModel>
    {
        public bool Equals(SimpleWorkflowModel x, SimpleWorkflowModel y)
        {
            if (ReferenceEquals(x, y))
                return true;

            return Equals(x.Id, y.Id)
                   && Equals(x.Name, y.Name)
                   && Equals(x.StateId, y.StateId)
                   && Equals(x.StateName, y.StateName)
                   && Equals(x.FinalState, y.FinalState);
        }

        public int GetHashCode(SimpleWorkflowModel obj)
        {
            return 0;
        }
    }
}