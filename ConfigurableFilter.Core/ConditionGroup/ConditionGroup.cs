using System;
using System.Collections.Generic;
using ConfigurableFilters.Condition;

namespace ConfigurableFilters.ConditionGroup
{
    internal class ConditionGroup<TObject, TCondition>
        where TCondition : Enum
    {
        public List<Condition<TCondition, TObject>> Conditions { get; set; } = new();
    }
}
