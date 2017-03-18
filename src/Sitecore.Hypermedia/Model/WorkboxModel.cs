using System.Collections.Generic;

namespace Sitecore.Hypermedia.Model
{
    public class WorkboxModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public ICollection<WorkboxStateModel> States { get; set; }
    }
}