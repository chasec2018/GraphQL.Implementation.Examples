
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.HotChocolate.Types
{
    using Models.Data;

    public class GDPTotalDollarValueType : ObjectType<GDPTotalDollarValue>
    {

        protected override void Configure(IObjectTypeDescriptor<GDPTotalDollarValue> Descriptor)
        {
            
        }
    }
}
