using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ConfigurableValidationFilter.ValidationRules;

namespace ConfigurableValidationFilter
{
    public class ConfigurableValidationFilter<TObject, TRules>
        where TRules : Enum
    {
        #region (Constructor and properties)

        private readonly Dictionary<TRules, ValidationRule<TRules, TObject>> _validationRules = new();
        public readonly Dictionary<TRules, RuleMetadata<TRules>> MetaData = new();

        #endregion

        #region (Public API)

        public ValidationResult ApplyConfiguration(TObject obj, FilterConfiguration<TRules> filterConfiguration)
        {
            if (filterConfiguration.Groups.Count == 0 || filterConfiguration.Groups.All(group => group.Conditions.Count == 0))
                return new ValidationResult
                {
                    Success = false,
                    Error = "Filter: No conditions were specified"
                };

            var groupResults = new List<ValidationResult>();
            foreach (var groupConfiguration in filterConfiguration.Groups)
            {
                if (groupConfiguration.CountMin > groupConfiguration.CountMax)
                {
                    groupResults.Add(new ValidationResult
                    {
                        Error = groupConfiguration.GetModifierError(groupConfiguration.Name)
                    });
                    continue;
                }
                var conditionResults = new List<ValidationResult>();
                foreach (var conditionConfig in groupConfiguration.Conditions)
                {
                    ThrowIfNotConfigured(conditionConfig.Type);

                    var condition = _validationRules[conditionConfig.Type];
                    var result = condition.Validate(obj, conditionConfig.Params);
                    conditionResults.Add(result);
                }

                var groupSuccess = groupConfiguration.ApplyModifier(conditionResults);
                groupResults.Add(new ValidationResult
                {
                    Internal = conditionResults,
                    Success = groupSuccess,
                    Error = groupSuccess ? null : groupConfiguration.GetModifierError(groupConfiguration.Name)
                });
            }

            var filterSuccess = filterConfiguration.ApplyModifier(groupResults);
            return new ValidationResult
            {
                Internal = groupResults,
                Success = filterSuccess,
                Error = filterSuccess ? null : filterConfiguration.GetModifierError("Filter")
            };
        }

        public void UseCondition<TProperty, TConditionParams>(
            RuleMetadata<TRules> ruleMetadata,
            Func<TObject, TProperty> valueProvider,
            Func<TProperty, TConditionParams, bool> comparisonFunc
            )
        where TConditionParams : RuleParameters
        {
            var conditionType = ruleMetadata.RuleType;
            ThrowIfConfigured(conditionType);

            var comparisonFuncGeneric = GetGenericComparisonFunc(comparisonFunc);
            ruleMetadata.ParamType = typeof(TConditionParams).Name;
            var condition = new ValidationRule<TRules, TObject, TProperty>(valueProvider, comparisonFuncGeneric, ruleMetadata);
            _validationRules.Add(conditionType, condition);
            MetaData.Add(conditionType, ruleMetadata);
        }

        #endregion

        #region (Helpers)

        private bool ConditionIsConfigured(TRules _) => _validationRules.ContainsKey(_);

        private void ThrowIfConfigured(TRules _)
        {
            if (!ConditionIsConfigured(_)) return;

            throw new InvalidEnumArgumentException($"ValidationRule {_} is already configured.");
        }

        private void ThrowIfNotConfigured(TRules _)
        {
            if (ConditionIsConfigured(_)) return;

            throw new InvalidEnumArgumentException($"ValidationRule {_} is not configured.");
        }

        private static Func<TProperty, RuleParameters, bool> GetGenericComparisonFunc<TProperty, TConditionParams>(
            Func<TProperty, TConditionParams, bool> function)
            where TConditionParams : RuleParameters
            => (statType, comparisonParams) => function(statType, comparisonParams as TConditionParams);

        #endregion
    }
}
