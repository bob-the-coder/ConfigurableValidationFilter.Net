using System;

namespace ConfigurableFilters.Condition
{
    internal abstract class Condition<TCondition, TObject>
    where TCondition : Enum
    {
        public ConditionMetadata<TCondition> Metadata { get; set; }
        public abstract ConditionResult Compare(TObject obj, ConditionParams conditionParams);
    }

    internal sealed class Condition<TCondition, TObject, TProperty> : Condition<TCondition, TObject>
    where TCondition : Enum
    {
        private readonly Func<TObject, TProperty> _valueProvider;
        private readonly Func<TProperty, ConditionParams, bool> _comparator;

        public Condition(
            Func<TObject, TProperty> valueProvider,
            Func<TProperty, ConditionParams, bool> comparator,
            ConditionMetadata<TCondition> metaData)
        {
            _valueProvider = valueProvider;
            _comparator = comparator;
            Metadata = metaData;
        }

        public override ConditionResult Compare(TObject obj, ConditionParams conditionParams)
        {
            var value = _valueProvider(obj);
            var success = _comparator(value, conditionParams);
            return new ConditionResult
            {
                Success = success,
                ValidatedValue = value.ToString(),
                Error = success
                    ? null : (conditionParams is ConditionParamsWithError withError) 
                        ? withError.Error : "Comparison failed."
            };
        }
    }
}
