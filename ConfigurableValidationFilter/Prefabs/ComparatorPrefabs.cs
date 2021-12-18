using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConfigurableValidationFilter.Prefabs
{
    public static class ComparatorPrefabs
    {
        public static Func<int, IntMinMaxParams, bool> IntMinMax => (value, minMax) =>
            (!minMax.Min.HasValue || minMax.Min <= value) &&
            (!minMax.Max.HasValue || minMax.Max >= value);

        public static Func<int, IntListParam, bool> IntIsInList => (value, listParam) =>
            listParam.Value?.Length > 0 && new List<int>(listParam.Value).Contains(value);

        public static Func<int, IntValueParam, bool> IntEquals => (intValue, valueParam) =>
            intValue == valueParam.Value;

        public static Func<double, DoubleMinMaxParams, bool> DoubleMinMax => (value, minMax) =>
            (!minMax.Min.HasValue || minMax.Min < value) &&
            (!minMax.Max.HasValue || minMax.Max > value);

        public static Func<DateTime, DateTimeMinMaxParams, bool> DateTimeMinMax => (value, minMax) =>
            (!minMax.Min.HasValue || minMax.Min < value) &&
            (!minMax.Max.HasValue || minMax.Max > value);

        public static Func<DateTime, DateTimeValueParam, bool> DateEquals => (value, valueParam) =>
            value == valueParam.Value;

        public static Func<bool, BoolValueParam, bool> BoolEquals => (value, valueParam) =>
            value == valueParam.Value;

        public static Func<string, StringListParam, bool> StringIsInList => (value, listParam) =>
            listParam.Value?.Length > 0 && new List<string>(listParam.Value).Contains(value);
        public static Func<string, StringValueParam, bool> StringEquals => (value, valueParam) =>
            value.Equals(valueParam.Value, StringComparison.Ordinal);

        public static Func<string, StringValueParam, bool> StringMatchesRegex => (value, valueParam) =>
            !string.IsNullOrWhiteSpace(valueParam.Value) && Regex.IsMatch(value, valueParam.Value);
    }
}
