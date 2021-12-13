using System.Collections.Generic;
using BoxFilterExample;
using ConfigurableFilters.Condition;
using ConfigurableFilters.Prefabs;

namespace BoxFilter.Test
{
    public static class BoxFilters
    {
        public static FilterConfiguration<BoxCondition> SmallAndLight => new()
        {
            Modifier = ConditionGroupModifier.All,
            Groups = new List<ConditionGroupConfiguration<BoxCondition>>
            {
                new()
                {
                    Modifier = ConditionGroupModifier.All,
                    Conditions = new List<ConditionConfiguration<BoxCondition>>
                    {
                        new()
                        {
                            Type = BoxCondition.HeightBetween,
                            Params = new IntMinMaxParams {Max = 100, Error = "Too tall."}
                        },
                        new()
                        {
                            Type = BoxCondition.WeightBetween,
                            Params = new IntMinMaxParams {Max = 1000, Error = "Too heavy."}
                        }
                    }
                }
            }
        };

        public static FilterConfiguration<BoxCondition> TallAndHeavy => new()
        {
            Modifier = ConditionGroupModifier.All,
            Groups = new List<ConditionGroupConfiguration<BoxCondition>>
            {
                new()
                {
                    Modifier = ConditionGroupModifier.All,
                    Conditions = new List<ConditionConfiguration<BoxCondition>>
                    {
                        new()
                        {
                            Type = BoxCondition.HeightBetween,
                            Params = new IntMinMaxParams {Min = 151, Error = "Too short."}
                        },
                        new()
                        {
                            Type = BoxCondition.WeightBetween,
                            Params = new IntMinMaxParams {Min = 5001, Error = "Too light."}
                        }
                    }
                }
            }
        };

        public static FilterConfiguration<BoxCondition> IsWarmColor => new()
        {
            Modifier = ConditionGroupModifier.All,
            Groups = new List<ConditionGroupConfiguration<BoxCondition>>
            {
                new()
                {
                    Modifier = ConditionGroupModifier.All,
                    Conditions = new List<ConditionConfiguration<BoxCondition>>
                    {
                        new()
                        {
                            Type = BoxCondition.ColorIsInList,
                            Params = new StringListParam
                            {
                                Value = new[] {"Red", "Yellow", "Orange"},
                                Error = "Not warm enough."
                            }
                        }
                    }
                }
            }
        };


        public static FilterConfiguration<BoxCondition> PlainBrownBox => new()
        {
            Modifier = ConditionGroupModifier.All,
            Groups = new List<ConditionGroupConfiguration<BoxCondition>>
            {
                new()
                {
                    Modifier = ConditionGroupModifier.All,
                    Conditions = new List<ConditionConfiguration<BoxCondition>>
                    {
                        new()
                        {
                            Type = BoxCondition.HeightBetween,
                            Params = new IntMinMaxParams {Min = 101, Max = 150, Error = "Too tall or too short."}
                        },
                        new()
                        {
                            Type = BoxCondition.WidthBetween,
                            Params = new IntMinMaxParams {Min = 101, Max = 150, Error = "Too thick or too thin."}
                        },
                        new()
                        {
                            Type = BoxCondition.DepthBetween,
                            Params = new IntMinMaxParams {Min = 101, Max = 150, Error = "Too deep or too shallow."}
                        },
                        new()
                        {
                            Type = BoxCondition.WeightBetween,
                            Params = new IntMinMaxParams {Min = 1001, Max = 5000, Error = "Too heavy or too light."}
                        },
                        new()
                        {
                            Type = BoxCondition.ColorEquals,
                            Params = new StringValueParam {Value = "Brown", Error = "The box is not brown."}
                        },
                        new()
                        {
                            Type = BoxCondition.IsRecent,
                            Params = new BoolValueParam {Value = true, Error = "The box has been sitting for a while."}
                        }
                    }
                }
            }
        };
    }
}
