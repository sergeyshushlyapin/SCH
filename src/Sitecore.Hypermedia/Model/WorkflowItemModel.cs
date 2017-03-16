using System.Collections.Generic;

namespace Sitecore.Hypermedia.Model
{
    public class WorkflowItemModel
    {
        public string Name { get; set; }

        public string Language { get; set; }

        public int Version { get; set; }

        public ICollection<LinkModel> Links { get; set; }
    }
}