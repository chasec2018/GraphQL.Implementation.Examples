using HotChocolate.Types;
using HotChocolate.Language;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunction.HotChocolate.Types
{
    using Models.Data;

    public class GDPGrowthRateType : ObjectType<GDPGrowthRate>
    {

        protected override void Configure(IObjectTypeDescriptor<GDPGrowthRate> Descriptor)
        {

        }
    }
}
