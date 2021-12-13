namespace BoxFilterExample
{
    public static class BoxMetadata
    {
        public static BoxConditionMetadata HeightBetween => new()
        {
            ConditionType = BoxCondition.HeightBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Height is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxConditionMetadata HeightEquals => new()
        {
            ConditionType = BoxCondition.HeightEquals,
            UiParamType = UiParamType.Single,
            Name = "Height equals",
            Description = BoxDescriptions.NumberEquals
        };
        public static BoxConditionMetadata WidthBetween => new()
        {
            ConditionType = BoxCondition.WidthBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Width is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxConditionMetadata WidthEquals => new()
        {
            ConditionType = BoxCondition.WidthEquals,
            UiParamType = UiParamType.Single,
            Name = "Width equals",
            Description = BoxDescriptions.NumberEquals
        };
        public static BoxConditionMetadata DepthBetween => new()
        {
            ConditionType = BoxCondition.DepthBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Depth is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxConditionMetadata DepthEquals => new()
        {
            ConditionType = BoxCondition.DepthEquals,
            UiParamType = UiParamType.Single,
            Name = "Depth equals",
            Description = BoxDescriptions.NumberEquals
        };
        public static BoxConditionMetadata WeightBetween => new()
        {
            ConditionType = BoxCondition.WeightBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Weight is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxConditionMetadata WeightEquals => new()
        {
            ConditionType = BoxCondition.WeightEquals,
            UiParamType = UiParamType.Single,
            Name = "Weight equals",
            Description = BoxDescriptions.NumberEquals
        };
        public static BoxConditionMetadata AreaBetween => new()
        {
            ConditionType = BoxCondition.AreaBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Area is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxConditionMetadata VolumeBetween => new()
        {
            ConditionType = BoxCondition.VolumeBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Volume is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxConditionMetadata DensityBetween => new()
        {
            ConditionType = BoxCondition.DensityBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Density is between",
            Description = BoxDescriptions.NumberBetweenMinMax
        };
        public static BoxConditionMetadata ReceivedBetween => new()
        {
            ConditionType = BoxCondition.ReceivedBetween,
            UiParamType = UiParamType.MinMax,
            Name = "Box was received between",
            Description = BoxDescriptions.DateBetweenMinMax
        };
        public static BoxConditionMetadata ColorIsInList => new()
        {
            ConditionType = BoxCondition.ColorIsInList,
            UiParamType = UiParamType.Single,
            Name = "Color is one of the following",
            Description = BoxDescriptions.ItemIsInList
        };
        public static BoxConditionMetadata ColorEquals => new()
        {
            ConditionType = BoxCondition.ColorEquals,
            UiParamType = UiParamType.Single,
            Name = "Color is",
            Description = BoxDescriptions.StringEquals
        };
        public static BoxConditionMetadata ColorMatchesRegex => new()
        {
            ConditionType = BoxCondition.ColorMatchesRegex,
            UiParamType = UiParamType.Single,
            Name = "Color name matches this Regex",
            Description = BoxDescriptions.StringEquals
        };

        public static BoxConditionMetadata IsRecent => new()
        {
            ConditionType = BoxCondition.IsRecent,
            UiParamType = UiParamType.Checkbox,
            Name = "Box is recent (1 day)",
            Description = BoxDescriptions.BoolEquals
        };
    }
}
