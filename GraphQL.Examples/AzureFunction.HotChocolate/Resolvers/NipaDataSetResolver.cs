using System;
using System.Net.Mime;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;


namespace AzureFunction.HotChocolate.Resolvers
{
    using Enums;
    using Services;
    using Models;
    using Models.Data;


    public class NipaDataSetResolver
    {
        private readonly HttpClient Client;
        private readonly RequestService RequestBuilder;
        private readonly DeserializerService DeserializerService;

        public NipaDataSetResolver(HttpClient client, RequestService requestBuilder, DeserializerService deserializerService)
        {
            Client = client;
            RequestBuilder = requestBuilder;
            DeserializerService = deserializerService;
        }


        public async Task<IQueryable<GDPGrowthRate>> GetGDPGrowthRateAsync(Frequency frequency, int From, int To)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            RequestBuilder.AddDataSet(DataSet.NIPA);
            RequestBuilder.AddTableName(TableName.T10101);
            RequestBuilder.AddTimePeriodRange(From,To);
            RequestBuilder.AddFrequency(frequency);


            Response<GDPGrowthRate> Results = JsonSerializer.Deserialize<Response<GDPGrowthRate>>(
                await Client.GetStringAsync(RequestBuilder.Request), DeserializerService.GetJsonSerializerOptions());

            Results.BeaApi.Results.Data.ForEach(x =>
            {
                x.Decade = int.Parse(string.Concat(x.TimePeriod.Substring(0, 3), "0"));
                x.Year = int.Parse(x.TimePeriod.Substring(0, 4));
                x.Quarter = frequency == Frequency.Quarterly ?  int.Parse(x.TimePeriod.Substring(5, 1)) : 0;
            });


            return Results.BeaApi.Results.Data.AsQueryable();

        }

        public async Task<IQueryable<GDPTotalDollarValue>> GetGDPTotalDollarValueAsync(Frequency frequency, int From, int To)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            RequestBuilder.AddDataSet(DataSet.NIPA);
            RequestBuilder.AddTableName(TableName.T10105);
            RequestBuilder.AddTimePeriodRange(From, To);
            RequestBuilder.AddFrequency(frequency);


            Response<GDPTotalDollarValue> Results = JsonSerializer.Deserialize<Response<GDPTotalDollarValue>>(
                await Client.GetStringAsync(RequestBuilder.Request), DeserializerService.GetJsonSerializerOptions());

            Results.BeaApi.Results.Data.ForEach(x =>
            {
                x.Decade = int.Parse(string.Concat(x.TimePeriod.Substring(0, 3), "0"));
                x.Year = int.Parse(x.TimePeriod.Substring(0, 4));
                x.Quarter = frequency == Frequency.Quarterly ? int.Parse(x.TimePeriod.Substring(5, 1)) : 0;
                x.MetricValue *= 1000000;
            });

            return Results.BeaApi.Results.Data.AsQueryable();
        }


        public async Task<IQueryable<GDPTotalDollarValue>> GetGDPPriceIndexesAsync(Frequency frequency, int From, int To)
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            RequestBuilder.AddDataSet(DataSet.NIPA);
            RequestBuilder.AddTableName(TableName.T10104);
            RequestBuilder.AddTimePeriodRange(From, To);
            RequestBuilder.AddFrequency(frequency);

            Response<GDPTotalDollarValue> Results = JsonSerializer.Deserialize<Response<GDPTotalDollarValue>>(
                await Client.GetStringAsync(RequestBuilder.Request), DeserializerService.GetJsonSerializerOptions());

            Results.BeaApi.Results.Data.ForEach(x =>
            {
                x.Decade = int.Parse(string.Concat(x.TimePeriod.Substring(0, 3), "0"));
                x.Year = int.Parse(x.TimePeriod.Substring(0, 4));
                x.Quarter = frequency == Frequency.Quarterly ? int.Parse(x.TimePeriod.Substring(5, 1)) : 0;
                x.MetricValue *= 1000000;
            });

            return Results.BeaApi.Results.Data.AsQueryable();
        }
    }
}
