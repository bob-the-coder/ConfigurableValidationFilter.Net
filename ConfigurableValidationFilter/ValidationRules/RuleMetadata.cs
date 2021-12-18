using System;

namespace ConfigurableValidationFilter.ValidationRules
{
    public abstract class RuleMetadata<TRules>
    {
        public TRules RuleType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParamType { get; internal set; }
    }
}
