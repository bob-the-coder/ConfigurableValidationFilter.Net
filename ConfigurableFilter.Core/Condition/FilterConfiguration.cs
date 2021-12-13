using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurableFilters.Condition
{
    public abstract class ConfigurationWithModifier<TCondition>
    {
        public ConditionGroupModifier Modifier { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }

        public bool ApplyModifier(ICollection<ValidationResult> results)
        {
            var successfulCount = results.Count(_ => _.Success);

            return Modifier == ConditionGroupModifier.All && successfulCount == results.Count ||
                   Modifier == ConditionGroupModifier.Any && successfulCount > 0 ||
                   Modifier == ConditionGroupModifier.Count &&
                   CountMin <= successfulCount && successfulCount <= CountMax;
        }

        public string GetModifierError()
        {
            return Modifier switch
            {
                ConditionGroupModifier.All => "All conditions in this group should be true.",
                ConditionGroupModifier.Any => "At least one condition in this group should be true.",
                ConditionGroupModifier.Count when CountMin > CountMax =>
                    $"The minimum number of true conditions must be lower than the maximum number.",
                ConditionGroupModifier.Count =>
                    $"Between {CountMin} and {CountMax} conditions in this group should be true.",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public class FilterConfiguration<TCondition> : ConfigurationWithModifier<TCondition>
    where TCondition : Enum
    {
        public string Name { get; set; }
        public List<ConditionGroupConfiguration<TCondition>> Groups { get; set; } = new();
    }

    public class ConditionGroupConfiguration<TCondition> : ConfigurationWithModifier<TCondition>
    where TCondition : Enum
    {
        public List<ConditionConfiguration<TCondition>> Conditions { get; set; } = new();
    }

    public class ConditionConfiguration<TCondition>
    where TCondition : Enum
    {
        public TCondition Type { get; set; }
        public ConditionParams Params { get; set; }
    }
}
