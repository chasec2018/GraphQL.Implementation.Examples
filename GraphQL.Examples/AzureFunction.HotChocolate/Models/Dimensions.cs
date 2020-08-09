using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.HotChocolate.Models
{
    public class Dimensions
    {
        public string Ordinal { get; set; }
        public string Name { get; set; }
        public string  DataType { get; set; }
        public string IsValue { get; set; }
    }
}
