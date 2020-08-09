using System;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureFunction.HotChocolate.Startup))]
namespace AzureFunction.HotChocolate
{
    using Roots;
    using Types;
    using Services;
    using Middleware;
    using Resolvers;

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder Builder)
        {
            // Add Services
            Builder.Services.AddTransient<RequestService>();
            Builder.Services.AddTransient<DeserializerService>();
            Builder.Services.AddHttpClient<NipaDataSetResolver>();

            // Add Field Resolvers
            Builder.Services.AddTransient<NipaDataSetResolver>();
            Builder.Services.AddTransient<FixedAssetsDataSetResolver>();

            // Create & Add Schema Services
            Builder.Services.AddGraphQL(ServiceProvider =>
                SchemaBuilder.New()
                    // Add Root ObjectTypes
                    .AddQueryType<GatewayFields>()

                    // Add Return Types
                    .AddType<GDPGrowthRateType>()
                    .AddType<GDPPriceIndexesType>()

                    // Do NOT UnCommit the below line
                    //.AddServices(ServiceProvider) <-- Unlike a traditional ASP.NET Core Application where Services are Built, Configured, and Run at 
                    //                                  Startup Azure Function Apps have services being regirstered before and after Function Startup.
                    //                                  As a result we need to ensure all the service are availble within the IoC Container  to add our ServiceProvider containing our Service Collection at Execution time.
                    //                                  You can see this demostraited in HotChocolateGraphHttpRequestHandler class under the Middleware folder
                    //                                  on Line 72. (Note: Based on HttpClientFactory DI Resolve Issue)
                    .Create(),                                                  
                    new QueryExecutionOptions 
                    { 
                        ForceSerialExecution = true,
                        IncludeExceptionDetails = true
                    });

            // Add the Execution Middleware
            Builder.Services.AddGraphQueryExeutionMiddleware();
        }
    }
}
