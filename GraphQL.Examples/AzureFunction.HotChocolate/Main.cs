using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;


namespace AzureFunction.HotChocolate
{
    using Middleware;

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
}
