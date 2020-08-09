
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotChocolate.Language;
using HotChocolate.Execution;
using System.Collections.Generic;

namespace AzureFunction.HotChocolate.Middleware
{
    public interface IHotChocolateGraphHttpRequestHandler
    {
        IHotChocolateGraphMiddlewareOptions GraphFunctionOptions { get; }
        IDocumentCache DocumentCache { get; }
        IDocumentHashProvider DocumentHashProvider { get; }
        IQueryExecutor Executor { get; }
        IReadOnlyList<GraphQLRequest> GraphQueryRequests { get; set; }
        Task<IActionResult> ExecuteFunctionsQueryAsync(HttpContext Context, CancellationToken StopingToken);
    }
}
