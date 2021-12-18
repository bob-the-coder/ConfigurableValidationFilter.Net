namespace BoxFilterExample
{
    public static class BoxMetadata
    {
        public static BoxRuleMetadata HeightBetween => new()
        {
            ConditionType = BoxRules.HeightBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Height is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxRuleMetadata HeightEquals => new()
        {
            ConditionType = BoxRules.HeightEquals,
            UiParamType = UiParamType.Single,
            Name = "Height equals",
            Description = BoxDescriptions.NumberEquals
        };
        public static BoxRuleMetadata WidthBetween => new()
        {
            ConditionType = BoxRules.WidthBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Width is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxRuleMetadata WidthEquals => new()
        {
            ConditionType = BoxRules.WidthEquals,
            UiParamType = UiParamType.Single,
            Name = "Width equals",
            Description = BoxDescriptions.NumberEquals
        };
        public static BoxRuleMetadata DepthBetween => new()
        {
            ConditionType = BoxRules.DepthBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Depth is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxRuleMetadata DepthEquals => new()
        {
            ConditionType = BoxRules.DepthEquals,
            UiParamType = UiParamType.Single,
            Name = "Depth equals",
            Description = BoxDescriptions.NumberEquals
        };
        public static BoxRuleMetadata WeightBetween => new()
        {
            ConditionType = BoxRules.WeightBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Weight is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxRuleMetadata WeightEquals => new()
        {
            ConditionType = BoxRules.WeightEquals,
            UiParamType = UiParamType.Single,
            Name = "Weight equals",
            Description = BoxDescriptions.NumberEquals
        };
        public static BoxRuleMetadata AreaBetween => new()
        {
            ConditionType = BoxRules.AreaBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Area is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxRuleMetadata VolumeBetween => new()
        {
            ConditionType = BoxRules.VolumeBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Volume is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxRuleMetadata DensityBetween => new()
        {
            ConditionType = BoxRules.DensityBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Density is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxRuleMetadata ReceivedBetween => new()
        {
            ConditionType = BoxRules.ReceivedBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Box was received between",
            Description = BoxDescriptions.DateBetweenMinMax
        };
        public static BoxRuleMetadata ColorIsInList => new()
        {
            ConditionType = BoxRules.ColorIsInList,
            UiParamType = UiParamType.Single,
            Name = "Color is one of the following",
            Description = BoxDescriptions.ItemIsInList
        };
        public static BoxRuleMetadata ColorEquals => new()
        {
            ConditionType = BoxRules.ColorEquals,
            UiParamType = UiParamType.Single,
            Name = "Color is",
            Description = BoxDescriptions.StringEquals
        };
        public static BoxRuleMetadata ColorMatchesRegex => new()
        {
            ConditionType = BoxRules.ColorMatchesRegex,
            UiParamType = UiParamType.Single,
            Name = "Color name matches this Regex",
            Description = BoxDescriptions.StringEquals
        };

        public static BoxRuleMetadata IsRecent => new()
        {
            ConditionType = BoxRules.IsRecent,
            UiParamType = UiParamType.Checkbox,
            Name = "Box is recent (1 day)",
            Description = BoxDescriptions.BoolEquals
        };
    }
}
