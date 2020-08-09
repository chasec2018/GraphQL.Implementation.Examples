
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureFunction.HotChocolate.Services
{
    public class DeserializerService
    {
        public JsonSerializerOptions GetJsonSerializerOptions()
        {
            var Options = new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                WriteIndented = true
            };

            Options.Converters.Add(new FloatConverter());
            Options.Converters.Add(new Int64Converter());

            return Options;
        }

        internal partial class FloatConverter : JsonConverter<float>
        {
            public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                float.Parse(reader.GetString());

            public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options) =>
                Write(writer, value, options);
        }

        internal partial class Int64Converter : JsonConverter<Int64>
        {
            public override Int64 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                Int64.Parse(reader.GetString().Replace(",", ""));

            public override void Write(Utf8JsonWriter writer, Int64 value, JsonSerializerOptions options) =>
                Write(writer, value, options);
        }
    }
}
