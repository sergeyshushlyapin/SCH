using Newtonsoft.Json;
using Sitecore.Hypermedia.Converters;

namespace Sitecore.Hypermedia.Model
{
    [JsonConverter(typeof(LinkModelConverter))]
    public class LinkModel
    {
        public string Href { get; set; }

        public string Rel { get; set; }

        public string Method { get; set; }
    }
}