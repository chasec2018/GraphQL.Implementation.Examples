using HotChocolate.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.HotChocolate.Middleware
{
    public interface IHotChocolateGraphMiddlewareOptions : IParserOptionsAccessor
    {
        int MaxRequestSize { get; }
    }
}
