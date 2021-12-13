using ConfigurableFilters;
using ConfigurableFilters.Prefabs;

namespace BoxFilterExample
{
    public class ConfigurableBoxFilter : ConfigurableFilter<Box, BoxCondition>
    {
        public static ConfigurableBoxFilter Default()
        {
            var filter = new ConfigurableBoxFilter();
            
            filter.UseCondition(BoxMetadata.HeightBetween, BoxValueProviders.Height, ComparatorPrefabs.IntMinMax);
            filter.UseCondition(BoxMetadata.HeightEquals, BoxValueProviders.Height, ComparatorPrefabs.IntEquals);
            filter.UseCondition(BoxMetadata.WidthBetween, BoxValueProviders.Width, ComparatorPrefabs.IntMinMax);
            filter.UseCondition(BoxMetadata.WidthEquals, BoxValueProviders.Width, ComparatorPrefabs.IntEquals);
            filter.UseCondition(BoxMetadata.DepthBetween, BoxValueProviders.Depth, ComparatorPrefabs.IntMinMax);
            filter.UseCondition(BoxMetadata.DepthEquals, BoxValueProviders.Depth, ComparatorPrefabs.IntEquals);
            filter.UseCondition(BoxMetadata.WeightBetween, BoxValueProviders.Weight, ComparatorPrefabs.IntMinMax);
            filter.UseCondition(BoxMetadata.WeightEquals, BoxValueProviders.Weight, ComparatorPrefabs.IntEquals);
            filter.UseCondition(BoxMetadata.AreaBetween, BoxValueProviders.Area, ComparatorPrefabs.IntMinMax);
            filter.UseCondition(BoxMetadata.VolumeBetween, BoxValueProviders.Volume, ComparatorPrefabs.IntMinMax);
            filter.UseCondition(BoxMetadata.DensityBetween, BoxValueProviders.Density, ComparatorPrefabs.DoubleMinMax);
            filter.UseCondition(BoxMetadata.ReceivedBetween, BoxValueProviders.ReceivedOn, ComparatorPrefabs.DateTimeMinMax);
            filter.UseCondition(BoxMetadata.ColorIsInList, BoxValueProviders.Color, ComparatorPrefabs.StringIsInList);
            filter.UseCondition(BoxMetadata.ColorEquals, BoxValueProviders.Color, ComparatorPrefabs.StringEquals);
            filter.UseCondition(BoxMetadata.ColorMatchesRegex, BoxValueProviders.Color, ComparatorPrefabs.StringMatchesRegex);
            filter.UseCondition(BoxMetadata.IsRecent, BoxValueProviders.IsRecent, ComparatorPrefabs.BoolEquals);

            return filter;
        }
    }
}
