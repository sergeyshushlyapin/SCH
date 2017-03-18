using System.Collections.Generic;

namespace Sitecore.Hypermedia.Model
{
    public class WorkboxStateModel
    {
        public string Name { get; set; }

        public bool FinalState { get; set; }

        public IEnumerable<WorkflowItemModel> Items { get; set; }

        public ICollection<LinkModel> Links { get; set; }
    }
}