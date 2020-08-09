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
using HotChocolate.AspNetCore;

namespace AzureFunction.HotChocolate.Middleware
{
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
}
