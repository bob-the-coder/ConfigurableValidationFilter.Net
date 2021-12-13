using System;

namespace ConfigurableFilters.Condition
{
    public abstract class ConditionMetadata<TCondition>
    where TCondition : Enum
    {
        public TCondition ConditionType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParamType { get; internal set; }
    }
}
