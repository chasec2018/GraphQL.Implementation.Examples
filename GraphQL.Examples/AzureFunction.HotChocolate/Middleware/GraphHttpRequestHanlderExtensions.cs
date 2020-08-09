using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;


namespace AzureFunction.HotChocolate.Middleware
{
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
}
