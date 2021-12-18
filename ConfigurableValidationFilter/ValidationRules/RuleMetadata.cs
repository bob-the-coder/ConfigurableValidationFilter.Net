using System;

namespace ConfigurableValidationFilter.ValidationRules
{
    public abstract class RuleMetadata<TCondition>
    where TCondition : Enum
    {
        public TCondition ConditionType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParamType { get; internal set; }
    }
}
