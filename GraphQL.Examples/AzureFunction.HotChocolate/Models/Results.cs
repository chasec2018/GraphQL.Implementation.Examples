using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AzureFunction.HotChocolate.Models
{
    public class Results<TData>
    {
        [JsonPropertyName("Statistic")]
        public string Statistic { get; set; }

        [JsonPropertyName("UTCProductionTime")]
        public DateTime? UtcProductionTime { get; set; }

        [JsonPropertyName("Dimensions")]
        public List<Dimensions> Dimensions { get; set; }

        [JsonPropertyName("Data")]
        public List<TData> Data { get; set; }

    }
}
