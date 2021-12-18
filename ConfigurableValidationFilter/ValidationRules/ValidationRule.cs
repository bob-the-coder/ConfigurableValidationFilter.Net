using System;

namespace ConfigurableValidationFilter.ValidationRules
{
    internal abstract class ValidationRule<TCondition, TObject>
    where TCondition : Enum
    {
        public RuleMetadata<TCondition> Metadata { get; set; }
        public abstract ValidationResult Validate(TObject obj, RuleParameters ruleParameters);
    }

    internal sealed class ValidationRule<TCondition, TObject, TProperty> : ValidationRule<TCondition, TObject>
    where TCondition : Enum
    {
        private readonly Func<TObject, TProperty> _valueProvider;
        private readonly Func<TProperty, RuleParameters, bool> _comparator;

        public ValidationRule(
            Func<TObject, TProperty> valueProvider,
            Func<TProperty, RuleParameters, bool> comparator,
            RuleMetadata<TCondition> metaData)
        {
            _valueProvider = valueProvider;
            _comparator = comparator;
            Metadata = metaData;
        }

        public override ValidationResult Validate(TObject obj, RuleParameters ruleParameters)
        {
            var value = _valueProvider(obj);
            var success = _comparator(value, ruleParameters);
            var defaultError = $"{Metadata.Name} {ruleParameters}. Found value {value}.";
            return new ValidationResult
            {
                Success = success,
                ValidatedValue = value.ToString(),
                Error = success
                    ? null : ruleParameters is RuleParametersWithError withError 
                        ? withError.Error ?? defaultError : defaultError
            };
        }
    }
}
