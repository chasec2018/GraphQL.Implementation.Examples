using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunction.HotChocolate.Middleware;
using System.Threading;

namespace AzureFunction.HotChocolate
{
    public class Main
    {
        private readonly IHotChocolateGraphHttpRequestHandler RequestHandler;

        public Main(IHotChocolateGraphHttpRequestHandler requestHandler)
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
