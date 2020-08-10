## GraphQL Gateway Implementation Examples


### Project Overview
The current examples within this repository uses the HotChocolate Framework to implement a GraphQL Gateway within an HttpTriggered Azure Function and an ASP.NET Core Web Application. These examples will include scenarios which will consume data from services such as third-party APIs, relational database systems like SQL Server utilizing Entity Frameworks, and non-relational document based systems like CosmoDb.

#### References
It's recommended that you read the following documentation and have a strong understanding of the listed references before jumping into the source code.  

- [Fundamentals on ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1)
- [Fundamentals on ASP.NET Cre Dependency Injectio](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)
- [Dependency Injection in Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
- [🌶 HotChocolate Docs](https://hotchocolate.io)
- [🌶 Framework Repository](https://github.com/ChilliCream/hotchocolate)
- [🌶 Demo by HotChocolate Auther (Michael Staib)](https://dev.to/michaelstaib/get-started-with-hot-chocolate-and-entity-framework-e9i)

#### Data
The data being consumed is comming from the following sources
- BEA.gov APIs (and Open Source and free APIs) will need
### I. ++Azure Function Implementation [In-Progress]++
Unlike a traditional ASP.NET Core Application where we can configure the host at Startup and tell the application which endpoints to use, we need to build a sort of Middleware that can handle the execution of incoming request within the Http Pipline. The current example deomstraits how to implement a custom middleware for executing queries that are processed from the Http Pipline by parsing the request stream. 
![Implementation Mockup](https://github.com/chasec2018/GraphQL.Implementation.Examples/blob/features/initial_start/Assets/uml-query-middleware-diagram.png)
#### Deliverables
1. **Download Dependencies** ✓ Finished
    - HotChocolate
    - HotChocolate.AspNetCore
    - HotChocolate.Language
    - HotChocolate.Server
    - HotChocolate.Types
    - HotChocolate.Types.Filters
    - HotChocolate.Utilities
    - Microsoft.Azure.Functions.Extensions (This will allow us to enable dependency injection)
    - Microsoft.Extensions.Http (This will add Resiliance to our HttpClient so we don't run into Socket Exhaustion)
2. **Create a Startup Class which inherits from FunctionsStartup** ✓ Finished
   - Make sure to have the following attribute above your startup class namespace or none of the service will be added to IoC Container : [assembly: FunctionsStartup(typeof(YourNamespace.Startup))]
        ```csharp
        [assembly: FunctionsStartup(typeof(YourNamespace.Startup))]
        namespace YourNameSpace
        {
            public class Startup : FunctionsStartup
            {
                public override void Configure(IFunctionsHostBuilder Builder)
                {
                    // Step 01 : Add Services
                    Builder.Services.AddHttpClient<EndpointResolver>(); // Typed HttpClient will prevent Scocket Exhaustion

                    // Step 02 : Add Field Resolvers Services
                    Builder.Services.AddTransient<EndpointResolver>();

                    Builder.Services.AddDbContext<YourDbContext>(
                        Configure => Configure.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings:ConnectionStringKey")), ServiceLifeTime.Transient);


                    // Step 03 : Create & Build GraphQL Schema
                    Builder.Services.AddGraphQL(ServiceProvider =>
                        SchemaBuilder.New()
                            // Add Root ObjectTypes
                            .AddQueryType<GatewayFields>()
                            .AddMutationType<GatewayMutations>()
                            .AddSubscriptionType<GatewaySubscriptions>()
                    
                            // Add Return Types
                            .AddType<ReturnType>()

                            // Do Not Uncomment the below line 
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

                    // Step 04 : Add the Execution Middleware
                    Builder.Services.AddGraphQueryExeutionMiddleware();
                }
        
            }
        }
        ```
2. **Create Middleware for Query Execution** ✓ Finished
    - Create Interface for Request Handler
        ```csharp
        using System;
        using HotChocolate.Language;

        public interface IGraphHttpRequestHandler
        {
            JsonQueryResultSerializer ResultSerializer { get; set; }
            IReadOnlyList<GraphQLRequest> GraphQueryRequests { get; set; }
            Task<IActionResult> ExecuteFunctionsQueryAsync(HttpContext Context, CancellationToken StopingToken);
        }
        ```
    - Create interface for Request Handler Options
        ```csharp
        public interface IGraphHttpRequestHandlerOptions : IParserOptionsAccessor
        {
            int MaxRequestSize { get; }
        }
        ```
    - Implement Request Handler Options
        ```csharp
        public class GraphHttpRequestHandlerOptions : IGraphHttpRequestHandlerOptions
        {
            private int minRequestSize = 1024;
            private int maxRequestSize = 20 * 1024 * 1024;
            private ParserOptions parserOptions = new ParserOptions();


            public ParserOptions ParserOptions
            {
                get { return parserOptions; }
                set
                {
                    if (value == null)
                        throw new ArgumentNullException(nameof(value));

                    parserOptions = value;
                }
            }

            public int MaxRequestSize
            {
                get { return maxRequestSize; }
                set
                {
                    if (value < minRequestSize)
                        throw new ArgumentException("Then minimum request size is 1024 bytes.", nameof(value));
                    maxRequestSize = value;
                }
            }
        }
        ```
    - Implement Http Request Handler 
        ```csharp
         public class GraphHttpRequestHandler : RequestHelper, IGraphHttpRequestHandler
         {
            private readonly IQueryExecutor Executor;
            private readonly IServiceProvider ServiceProvider;


            public GraphHttpRequestHandler(IServiceProvider serviceProvider, IQueryExecutor queryExecutor, IDocumentCache documentCache, IDocumentHashProvider documentHashProvider, IGraphHttpRequestHandlerOptions graphHttpRequestHanlderOptions) 
                : base(documentCache, documentHashProvider, graphHttpRequestHanlderOptions.MaxRequestSize, graphHttpRequestHanlderOptions.ParserOptions)
            {
                Executor = queryExecutor;
                ServiceProvider = serviceProvider;
            }


            public JsonQueryResultSerializer ResultSerializer { get; set; }
            public IReadOnlyList<GraphQLRequest> GraphQueryRequests { get; set; }


            public async Task<IActionResult> ExecuteFunctionsQueryAsync(HttpContext Context, CancellationToken StopingToken)
            {
                ResultSerializer = new JsonQueryResultSerializer();

                using (Stream Stream = Context.Request.Body)
                {
                    if (Context.Request.ContentType.Equals(MediaTypeNames.Application.Json))
                        GraphQueryRequests =
                            await base.ReadJsonRequestAsync(Stream, StopingToken).ConfigureAwait(false);


                    else if (Context.Request.ContentType.Equals("application/graphql"))
                        GraphQueryRequests =
                            await base.ReadGraphQLQueryAsync(Stream, StopingToken).ConfigureAwait(false);

                    else
                        return new BadRequestObjectResult("There was either no Query or the Query was mal-formed");


                    if (GraphQueryRequests.Count > 0)
                    {
                        var QueryRequest =
                            QueryRequestBuilder.New()
                                .SetServices(ServiceProvider)
                                .SetQuery(GraphQueryRequests[0].Query)
                                .SetOperation(GraphQueryRequests[0].OperationName)
                                .SetQueryName(GraphQueryRequests[0].QueryName);


                        if (GraphQueryRequests[0].Variables != null && GraphQueryRequests[0].Variables.Count > 0)
                            QueryRequest.SetVariableValues(GraphQueryRequests[0].Variables);

                        IExecutionResult Result = await Executor.ExecuteAsync(
                            QueryRequest.Create());

                        await ResultSerializer.SerializeAsync(
                            (IReadOnlyQueryResult)Result, Context.Response.Body, StopingToken);

                    }

                    return new EmptyResult();
                }
            }
         }
        ```
    - Create & Expose IHttpRequestHandler to Service Container
        ```csharp
        public static class GraphHttpRequestHanlderExtensions
        {
            public static IServiceCollection AddGraphQueryExeutionMiddleware(this IServiceCollection ServiceCollection, IGraphHttpRequestHandlerOptions Options)
            {
                ServiceCollection.AddSingleton(Options);
                // Performance Note: Singleton slows down Resolvers. Transienct will allow concurent execution of multiple Fields
                ServiceCollection.AddTransient<IGraphHttpRequestHandler, GraphHttpRequestHandler>();

                return ServiceCollection;
            }

            public static IServiceCollection AddGraphQueryExeutionMiddleware(this IServiceCollection ServiceCollection)
            {
                ServiceCollection.AddGraphQueryExeutionMiddleware(new GraphHttpRequestHandlerOptions());
                return ServiceCollection;
            }
        }
        ```

3. **Inject & Implement IHttpRequestHandler into Function Endpoint**

     ```csharp 
     public class Main
     {
            private readonly IGraphHttpRequestHandler RequestHandler;

            public Main(IGraphHttpRequestHandler requestHandler)
            {
                RequestHandler = requestHandler;
            }


            [FunctionName("GraphGateway")]
            public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest Request, CancellationToken StoppingToken)
            {
                return await RequestHandler.ExecuteFunctionsQueryAsync(Request.HttpContext, StoppingToken);
            }
       }
 ```


### II. ++ASP.NET Core Web App Implementation [Not Finished]++
