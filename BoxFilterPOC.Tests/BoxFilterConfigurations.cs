using System.Collections.Generic;
using BoxFilterExample;
using ConfigurableValidationFilter;
using ConfigurableValidationFilter.Prefabs;
using ConfigurableValidationFilter.ValidationRules;

namespace BoxFilterPOC.Tests
{
    public static class BoxFilterConfigurations
    {
        public static FilterConfiguration<BoxRules> SmallAndLight => new()
        {
            Modifier = RuleGroupModifier.All,
            Groups = new List<ConditionGroupConfiguration<BoxRules>>
            {
                new()
                {
                    Modifier = RuleGroupModifier.All,
                    Conditions = new List<ConditionConfiguration<BoxRules>>
                    {
                        new()
                        {
                            Type = BoxRules.HeightBetween,
                            Params = new IntMinMaxParams {Max = 100, Error = "Too tall."}
                        },
                        new()
                        {
                            Type = BoxRules.WeightBetween,
                            Params = new IntMinMaxParams {Max = 1000, Error = "Too heavy."}
                        }
                    }
                }
            }
        };

        public static FilterConfiguration<BoxRules> TallAndHeavy => new()
        {
            Modifier = RuleGroupModifier.All,
            Groups = new List<ConditionGroupConfiguration<BoxRules>>
            {
                new()
                {
                    Modifier = RuleGroupModifier.All,
                    Conditions = new List<ConditionConfiguration<BoxRules>>
                    {
                        new()
                        {
                            Type = BoxRules.HeightBetween,
                            Params = new IntMinMaxParams {Min = 151, Error = "Too short."}
                        },
                        new()
                        {
                            Type = BoxRules.WeightBetween,
                            Params = new IntMinMaxParams {Min = 5001, Error = "Too light."}
                        }
                    }
                }
            }
        };

        public static FilterConfiguration<BoxRules> IsWarmColor => new()
        {
            Modifier = RuleGroupModifier.All,
            Groups = new List<ConditionGroupConfiguration<BoxRules>>
            {
                new()
                {
                    Modifier = RuleGroupModifier.All,
                    Conditions = new List<ConditionConfiguration<BoxRules>>
                    {
                        new()
                        {
                            Type = BoxRules.ColorIsInList,
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


        public static FilterConfiguration<BoxRules> PlainBrownBox => new()
        {
            Modifier = RuleGroupModifier.All,
            Groups = new List<ConditionGroupConfiguration<BoxRules>>
            {
                new()
                {
                    Modifier = RuleGroupModifier.All,
                    Conditions = new List<ConditionConfiguration<BoxRules>>
                    {
                        new()
                        {
                            Type = BoxRules.HeightBetween,
                            Params = new IntMinMaxParams {Min = 101, Max = 150, Error = "Too tall or too short."}
                        },
                        new()
                        {
                            Type = BoxRules.WidthBetween,
                            Params = new IntMinMaxParams {Min = 101, Max = 150, Error = "Too thick or too thin."}
                        },
                        new()
                        {
                            Type = BoxRules.DepthBetween,
                            Params = new IntMinMaxParams {Min = 101, Max = 150, Error = "Too deep or too shallow."}
                        },
                        new()
                        {
                            Type = BoxRules.WeightBetween,
                            Params = new IntMinMaxParams {Min = 1001, Max = 5000, Error = "Too heavy or too light."}
                        },
                        new()
                        {
                            Type = BoxRules.ColorEquals,
                            Params = new StringValueParam {Value = "Brown", Error = "The box is not brown."}
                        },
                        new()
                        {
                            Type = BoxRules.IsRecent,
                            Params = new BoolValueParam {Value = true, Error = "The box has been sitting for a while."}
                        }
                    }
                }
            }
        };
    }
}
