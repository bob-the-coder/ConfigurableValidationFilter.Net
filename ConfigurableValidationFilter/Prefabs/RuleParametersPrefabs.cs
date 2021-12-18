using System;
using ConfigurableValidationFilter.ValidationRules;

namespace ConfigurableValidationFilter.Prefabs
{
    public class ValueParam<TParam> : RuleParametersWithError
    {
        public TParam Value { get; set; }

        public override string ToString()
        {
            return Value == null ? "NONE" : Value.ToString();
        }
    }

    public class ListParam<TParam> : RuleParametersWithError
    {
        public TParam[] Value { get; set; }

        public override string ToString()
        {
            return Value?.Length > 0 ? string.Join(", ", Value) : "NONE";
        }
    }

    public class MinMaxParams<TParam> : RuleParametersWithError
    {
        public TParam Min { get; set; }
        public TParam Max { get; set; }

        private static string ValueOrAny(TParam param) => param == null ? "ANY" : param.ToString();

        public override string ToString()
        {
            return $"{ValueOrAny(Min)} and {ValueOrAny(Max)}";
        }
    }

    public class IntValueParam : ValueParam<int> { }
    public class StringValueParam : ValueParam<string> { }
    public class DateTimeValueParam : ValueParam<DateTime> { }
    public class BoolValueParam : ValueParam<bool> { }

    public class IntListParam : ListParam<int> { }
    public class StringListParam : ListParam<string> { }


    public class IntMinMaxParams : MinMaxParams<int?> { }
    public class DoubleMinMaxParams : MinMaxParams<double?> { }
    public class DateTimeMinMaxParams : MinMaxParams<DateTime?> { }
}
