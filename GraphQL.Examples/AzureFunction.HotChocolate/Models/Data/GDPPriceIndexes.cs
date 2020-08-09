using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AzureFunction.HotChocolate.Models.Data
{
    public class GDPPriceIndexes
    {
        [JsonPropertyName("TableName")]
        public string TableName { get; set; }

        [JsonPropertyName("SeriesCode")]
        public string SeriesCode { get; set; }

        [JsonPropertyName("LineNumber")]
        public string LineNumber { get; set; }

        [JsonPropertyName("LineDescription")]
        public string MetricDescription { get; set; }

        [JsonPropertyName("METRIC_NAME")]
        public string? MetricName { get; set; }

        [JsonPropertyName("TimePeriod")]
        public string TimePeriod { get; set; }

        public int? Decade { get; set; }

        public int? Year { get; set; }

        public int? Quarter { get; set; }

        [JsonPropertyName("CL_UNIT")]
        public string? UnitDescriptor { get; set; }

        [JsonPropertyName("UNIT_MULT")]
        public string? UnitMultiplier { get; set; }

        [JsonPropertyName("DataValue")]
        public float MetricValue { get; set; }
    }
}
