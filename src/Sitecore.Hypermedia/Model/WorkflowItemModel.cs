using System.Collections.Generic;

namespace Sitecore.Hypermedia.Model
{
    public class WorkflowItemModel
    {
        public string Name { get; set; }

        public string Language { get; set; }

        public int Version { get; set; }

        public IEnumerable<LinkModel> Links { get; set; }
    }
}