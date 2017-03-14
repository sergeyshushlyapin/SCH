using System;
using Newtonsoft.Json;
using Sitecore.Hypermedia.Model;

namespace Sitecore.Hypermedia.Converters
{
    public class LinkModelConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(LinkModel);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var model = value as LinkModel;
            if (model == null) return;

            writer.WriteStartObject();

            writer.WritePropertyName("href");
            writer.WriteValue(model.Href);

            writer.WritePropertyName("rel");
            writer.WriteValue(model.Rel);

            if (!model.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                writer.WritePropertyName("method");
                writer.WriteValue(model.Method);
            }

            writer.WriteEndObject();
        }
    }
}