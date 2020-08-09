using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AzureFunction.HotChocolate.Models
{
    public class Request
    {
        [JsonPropertyName("RequestParam")]
        public List<RequestParameters>? RequestParameters { get; set; }

    }
}
