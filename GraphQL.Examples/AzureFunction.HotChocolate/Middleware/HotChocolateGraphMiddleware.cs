using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.HotChocolate.Middleware
{
    public static class HotChocolateGraphMiddleware
    {
        public static IServiceCollection AddGraphQLExeutionMiddleware(this IServiceCollection ServiceCollection, IHotChocolateGraphMiddlewareOptions Options)
        {
            ServiceCollection.AddSingleton<IHotChocolateGraphMiddlewareOptions>(Options);

            // Performance Note: Singleton slows down Resolvers. Transienct will allow concurent execution of multiple Fields
            ServiceCollection.AddTransient<IHotChocolateGraphHttpRequestHandler, HotChocolateGraphHttpRequestHandler>();

            return ServiceCollection;
        }

        public static IServiceCollection AddGraphQLExeutionMiddleware(this IServiceCollection ServiceCollection)
        {
            ServiceCollection.AddGraphQLExeutionMiddleware(new HotChocolateGraphMiddlewareOptions());
            return ServiceCollection;
        }
    }
}
