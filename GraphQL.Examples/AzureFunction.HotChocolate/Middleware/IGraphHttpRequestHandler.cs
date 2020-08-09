using HotChocolate.Language;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotChocolate.Execution;

namespace AzureFunction.HotChocolate.Middleware
{
    public interface IGraphHttpRequestHandler
    {
        JsonQueryResultSerializer ResultSerializer { get; set; }
        IReadOnlyList<GraphQLRequest> GraphQueryRequests { get; set; }
        Task<IActionResult> ExecuteFunctionsQueryAsync(HttpContext Context, CancellationToken StopingToken);
    }
}
