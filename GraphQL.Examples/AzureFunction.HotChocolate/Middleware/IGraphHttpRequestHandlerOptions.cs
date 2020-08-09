using HotChocolate.AspNetCore;


namespace AzureFunction.HotChocolate.Middleware
{
    public interface IGraphHttpRequestHandlerOptions : IParserOptionsAccessor
    {
        int MaxRequestSize { get; }
    }
}
