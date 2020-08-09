using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AzureFunction.HotChocolate.Models
{
    public class Response<TData> where TData : class, new()
    {
        [JsonPropertyName("BEAAPI")]
        public Content<TData> BeaApi { get; set; }
    }
}
