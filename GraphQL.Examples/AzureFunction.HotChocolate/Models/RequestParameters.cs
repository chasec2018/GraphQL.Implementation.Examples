using System;
using System.Text.Json.Serialization;

namespace AzureFunction.HotChocolate.Models
{
    public class RequestParameters
    {
        [JsonPropertyName("ParameterName")]
        public string Name { get; set; }

        [JsonPropertyName("ParameterValue")]
        public string Value { get; set; }
    }
}
