using HotChocolate.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.HotChocolate.Middleware
{
    public class GraphHttpRequestHandlerOptions : IGraphHttpRequestHandlerOptions
    {
        private int minRequestSize = 1024;
        private int maxRequestSize = 20 * 1024 * 1024;

        public ParserOptions parserOptions = new ParserOptions();



        public ParserOptions ParserOptions
        {
            get { return parserOptions; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                parserOptions = value;
            }
        }

        public int MaxRequestSize
        {
            get { return maxRequestSize; }
            set
            {
                if (value < minRequestSize)
                    throw new ArgumentException("Then minimum request size is 1024 bytes.", nameof(value));
                maxRequestSize = value;
            }
        }
    }
}
