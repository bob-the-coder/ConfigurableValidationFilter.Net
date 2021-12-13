using Newtonsoft.Json;

namespace ConfigurableFilters.Condition
{
    [JsonConverter(typeof(DefaultConditionParamsJsonConverter))]
    public class ConditionParams
    {
    }
    public class ConditionParamsWithError : ConditionParams
    { 
        public string Error { get; set; }
    }
}
