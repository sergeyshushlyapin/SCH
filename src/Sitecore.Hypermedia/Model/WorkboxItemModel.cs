using System.Collections.Generic;

namespace Sitecore.Hypermedia.Model
{
    public class WorkboxItemModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public ICollection<LinkModel> Commands { get; set; }
    }
}