using System;
using ConfigurableFilters.Condition;

namespace ConfigurableFilters.Prefabs
{
    public class ValueParam<TParam> : ConditionParamsWithError
    {
        public TParam Value { get; set; }

        public override string ToString()
        {
            return Value == null ? "NONE" : Value.ToString();
        }
    }

    public class ListParam<TParam> : ConditionParamsWithError
    {
        public TParam[] Value { get; set; }

        public override string ToString()
        {
            return Value?.Length > 0 ? string.Join(", ", Value) : "NONE";
        }
    }

    public class MinMaxParams<TParam> : ConditionParamsWithError
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
