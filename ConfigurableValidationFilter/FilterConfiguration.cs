using System;
using System.Collections.Generic;
using System.Linq;
using ConfigurableValidationFilter.ValidationRules;

namespace ConfigurableValidationFilter
{
    public abstract class ConfigurationWithModifier
    {
        public RuleGroupModifier Modifier { get; set; }
        public int CountMin { get; set; }
        public int CountMax { get; set; }

        public bool ApplyModifier(ICollection<ValidationResult> results)
        {
            var successfulCount = results.Count(_ => _.Success);

            return Modifier == RuleGroupModifier.All && successfulCount == results.Count ||
                   Modifier == RuleGroupModifier.Any && successfulCount > 0 ||
                   Modifier == RuleGroupModifier.Count &&
                   CountMin <= successfulCount && successfulCount <= CountMax;
        }

        public string GetModifierError(string groupName)
        {
            var error = Modifier switch
            {
                RuleGroupModifier.All => "All conditions in this group should be true.",
                RuleGroupModifier.Any => "At least one condition in this group should be true.",
                RuleGroupModifier.Count when CountMin > CountMax =>
                    "The minimum number of conditions must be lower than or equal to the maximum.",
                RuleGroupModifier.Count when CountMin == CountMax =>
                    $"{CountMin} conditions in this group should be true.",
                RuleGroupModifier.Count =>
                    $"Between {CountMin} and {CountMax} conditions in this group should be true.",
                _ => throw new ArgumentOutOfRangeException()
            };

            return $"{groupName}: {error}";
        }
    }

    public class FilterConfiguration<TCondition> : ConfigurationWithModifier
    where TCondition : Enum
    {
        public string Name { get; set; }
        public List<ConditionGroupConfiguration<TCondition>> Groups { get; set; } = new();
    }

    public class ConditionGroupConfiguration<TCondition> : ConfigurationWithModifier
    where TCondition : Enum
    {
        public string Name { get; set; }
        public List<ConditionConfiguration<TCondition>> Conditions { get; set; } = new();
    }

    public class ConditionConfiguration<TCondition>
    where TCondition : Enum
    {
        public TCondition Type { get; set; }
        public RuleParameters Params { get; set; }
    }
}
