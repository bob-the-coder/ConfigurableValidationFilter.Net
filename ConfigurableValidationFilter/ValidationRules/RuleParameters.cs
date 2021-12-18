using Newtonsoft.Json;

namespace ConfigurableValidationFilter.ValidationRules
{
    [JsonConverter(typeof(RuleParametersJsonConverter))]
    public class RuleParameters
    {
    }

    public class RuleParametersWithError : RuleParameters
    { 
        public string Error { get; set; }
    }
}
