using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.HotChocolate.Models
{
    public class Content<TData>
    {
        public Request Request { get; set; }

        public Results<TData> Results { get; set; }

    }
}
