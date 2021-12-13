using System;
using System.Collections.Generic;

namespace ConfigurableFilters.Condition
{
    public class FilterConfiguration<TCondition>
    where TCondition : Enum
    {
        public string Name { get; set; }
        public List<ConditionConfiguration<TCondition>> Conditions { get; set; } = new();
    }

    public class ConditionConfiguration<TCondition>
    where TCondition : Enum
    {
        public TCondition Type { get; set; }
        public ConditionParams Params { get; set; }
    }
}
