using AzureFunction.HotChocolate.Models.Data;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.HotChocolate.Types
{
    public class GDPPriceIndexesType : ObjectType<GDPPriceIndexes>
    {

        protected override void Configure(IObjectTypeDescriptor<GDPPriceIndexes> Descriptor)
        {
            
        }
    }
}
