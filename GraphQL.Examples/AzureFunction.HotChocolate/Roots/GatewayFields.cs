
using System;
using HotChocolate.Types;

namespace AzureFunction.HotChocolate.Roots
{
    using Enums;
    using Types;
    using Resolvers;

    public class GatewayFields : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor Descriptor)
        {
            Descriptor.Name("BeaGovDataFields");

            Descriptor.Field("GDPGrowthRate")
                .Type<ListType<GDPGrowthRateType>>()
                .UseFiltering()
                .Argument("Frequency", filter => filter.DefaultValue(Frequency.Quarterly))
                .Argument("FromYear", filter => filter.DefaultValue(DateTime.Now.Year))
                .Argument("ToYear", filter => filter.DefaultValue(DateTime.Now.Year))
                .Resolver(Context =>
                    Context.Service<NipaDataSetResolver>().GetGDPGrowthRateAsync(
                        Context.Argument<Frequency>("Frequency"),
                        Context.Argument<int>("FromYear"),
                        Context.Argument<int>("ToYear")));

            Descriptor.Field("GDPTotalDollarValue")
                .Type<ListType<GDPTotalDollarValueType>>()
                .UseFiltering()
                .Argument("Frequency", filter => filter.DefaultValue(Frequency.Quarterly))
                .Argument("FromYear", filter => filter.DefaultValue(DateTime.Now.Year))
                .Argument("ToYear", filter => filter.DefaultValue(DateTime.Now.Year))
                .Resolver(Context =>
                    Context.Service<NipaDataSetResolver>().GetGDPTotalDollarValueAsync(
                        Context.Argument<Frequency>("Frequency"),
                        Context.Argument<int>("FromYear"),
                        Context.Argument<int>("ToYear")));

            Descriptor.Field("GDPPriceIndexes")
                .Type<ListType<GDPPriceIndexesType>>()
                .UseFiltering()
                .Argument("Frequency", filter => filter.DefaultValue(Frequency.Quarterly))
                .Argument("FromYear", filter => filter.DefaultValue(DateTime.Now.Year))
                .Argument("ToYear", filter => filter.DefaultValue(DateTime.Now.Year))
                .Resolver(Context =>
                    Context.Service<NipaDataSetResolver>().GetGDPPriceIndexesAsync(
                        Context.Argument<Frequency>("Frequency"),
                        Context.Argument<int>("FromYear"),
                        Context.Argument<int>("ToYear")));
        }
    }
}
