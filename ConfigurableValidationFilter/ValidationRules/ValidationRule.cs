using System;

namespace ConfigurableValidationFilter.ValidationRules
{
    internal abstract class ValidationRule<TRules, TObject>
    {
        public RuleMetadata<TRules> Metadata { get; set; }
        public abstract ValidationResult Validate(TObject obj, RuleParameters ruleParameters);
    }

    internal sealed class ValidationRule<TRules, TObject, TProperty> : ValidationRule<TRules, TObject>
    {
        private readonly Func<TObject, TProperty> _valueProvider;
        private readonly Func<TProperty, RuleParameters, bool> _comparator;

        public ValidationRule(
            Func<TObject, TProperty> valueProvider,
            Func<TProperty, RuleParameters, bool> comparator,
            RuleMetadata<TRules> metaData)
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
