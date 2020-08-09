using System;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Server;

namespace AzureFunction.HotChocolate.Middleware
{
    public class HotChocolateGraphHttpRequestHandler : IHotChocolateGraphHttpRequestHandler
    {

        private readonly IServiceProvider ServiceProvider;
        private readonly RequestHelper RequestHelper;
        private readonly JsonQueryResultSerializer Serializer;

        public HotChocolateGraphHttpRequestHandler(IServiceProvider serviceProvider, IQueryExecutor queryExecutor, IDocumentCache documentCache, IDocumentHashProvider documentHashProvider, IHotChocolateGraphMiddlewareOptions graphFunctionOptions)
        {
            ServiceProvider = serviceProvider;
            Executor = queryExecutor;
            DocumentCache = documentCache;
            DocumentHashProvider = documentHashProvider;
            GraphFunctionOptions = graphFunctionOptions;

            Serializer = new JsonQueryResultSerializer();

            RequestHelper = new RequestHelper(
              DocumentCache,
              DocumentHashProvider,
              GraphFunctionOptions.MaxRequestSize,
              GraphFunctionOptions.ParserOptions);
        }

        public IHotChocolateGraphMiddlewareOptions GraphFunctionOptions { get; }

        public IDocumentCache DocumentCache { get; }

        public IDocumentHashProvider DocumentHashProvider { get; }

        public IQueryExecutor Executor { get; }

        public IReadOnlyList<GraphQLRequest> GraphQueryRequests { get; set; }



        public async Task<IActionResult> ExecuteFunctionsQueryAsync(HttpContext Context, CancellationToken StopingToken)
        {
            using (Stream Stream = Context.Request.Body)
            {
                if (Context.Request.ContentType.Equals(MediaTypeNames.Application.Json))
                    GraphQueryRequests =
                        await RequestHelper.ReadJsonRequestAsync(Stream, StopingToken).ConfigureAwait(false);


                else if (Context.Request.ContentType.Equals("application/graphql"))
                    GraphQueryRequests =
                        await RequestHelper.ReadGraphQLQueryAsync(Stream, StopingToken).ConfigureAwait(false);

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

                    await Serializer.SerializeAsync(
                        (IReadOnlyQueryResult)Result, Context.Response.Body, StopingToken);

                }

                return new EmptyResult();
            }
        }
    }
}
