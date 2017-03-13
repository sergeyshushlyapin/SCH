using System.Collections.Generic;

namespace Sitecore.Hypermedia.Model
{
    public class WorkflowStateModel
    {
        public string Name { get; set; }
        public bool FinalState{ get; set; }

        public ICollection<LinkModel> Links { get; set; }
    }
}