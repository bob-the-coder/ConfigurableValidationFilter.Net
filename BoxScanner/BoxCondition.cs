using ConfigurableFilters.Condition;

namespace BoxFilterExample
{
    public enum BoxCondition
    {
        WeightBetween,
        WeightEquals,
        WidthBetween,
        WidthEquals,
        HeightBetween,
        HeightEquals,
        DepthBetween,
        DepthEquals,
        AreaBetween,
        VolumeBetween,
        DensityBetween,
        ReceivedBetween,
        ColorIsInList,
        ColorEquals,
        IsRecent,
        ColorMatchesRegex
    }

    public enum UiParamType
    {
        Single,
        MinMax,
        Checkbox
    }

    public class BoxConditionMetadata : ConditionMetadata<BoxCondition>
    {
        public UiParamType UiParamType { get; set; }
    }

    public static class BoxDescriptions
    {
        public const string NumberBetweenMinMax =
            "Checks that the value is between the provided limits (inclusive). Either Min or Max can be ommitted.";

        public const string NumberEquals =
            "Checks that the value is equal to the provided number. (Has 0.00001 error for floating-point numbers.)";

        public const string StringEquals =
            "Checks that the value is the same as the provided string. OrdinalCase comparison is used.";

        public const string DateEquals =
            "Checks that the value is the same as the provided date. The provided date has UTC+0 offset.";

        public const string BoolEquals =
            "Checks if the condition is explicitly true or false.";

        public const string DateBetweenMinMax =
            "Checks that the value is between the provided dates (inclusive). Either Min or Max can be ommitted. The provided dates have UTC+0 offset.";

        public const string StringMatchesPattern =
            "Checks that the value matches the provided regular expression. The regular expression is matched against the entire value.";

        public const string ItemIsInList =
            "Checks if the value is found in the provided list. Items in the provided list must be comma-delimited.";
    }
}
