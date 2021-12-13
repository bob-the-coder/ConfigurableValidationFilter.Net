using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ConfigurableFilters.Condition;

namespace ConfigurableFilters
{
    public class ConfigurableFilter<TObject, TCondition>
        where TCondition : Enum
    {
        #region (Constructor and properties)

        private readonly Dictionary<TCondition, Condition<TCondition, TObject>> _conditions = new();
        public readonly Dictionary<TCondition, ConditionMetadata<TCondition>> MetaData = new();

        #endregion

        #region (Public API)

        public ValidationResult ApplyConfiguration(TObject obj, FilterConfiguration<TCondition> filterConfiguration)
        {
            if (filterConfiguration.Groups.Count == 0 || filterConfiguration.Groups.All(group => group.Conditions.Count == 0))
                return new ValidationResult
                {
                    Success = false,
                    Error = "No conditions were specified"
                };

            var groupResults = new List<ValidationResult>();
            foreach (var groupConfiguration in filterConfiguration.Groups)
            {
                if (groupConfiguration.CountMin > groupConfiguration.CountMax)
                {
                    groupResults.Add(new ValidationResult
                    {
                        Error = groupConfiguration.GetModifierError()
                    });
                    continue;
                }
                var conditionResults = new List<ValidationResult>();
                foreach (var conditionConfig in groupConfiguration.Conditions)
                {
                    ThrowIfNotConfigured(conditionConfig.Type);

                    var condition = _conditions[conditionConfig.Type];
                    var result = condition.Validate(obj, conditionConfig.Params);
                    conditionResults.Add(result);
                }

                var groupSuccess = groupConfiguration.ApplyModifier(conditionResults);
                groupResults.Add(new ValidationResult
                {
                    Internal = conditionResults,
                    Success = groupSuccess,
                    Error = groupSuccess ? null : groupConfiguration.GetModifierError()
                });
            }

            var filterSuccess = filterConfiguration.ApplyModifier(groupResults);
            return new ValidationResult
            {
                Internal = groupResults,
                Success = filterSuccess,
                Error = filterSuccess ? null : filterConfiguration.GetModifierError()
            };
        }

        public void UseCondition<TProperty, TConditionParams>(
            ConditionMetadata<TCondition> conditionMetadata,
            Func<TObject, TProperty> valueProvider,
            Func<TProperty, TConditionParams, bool> comparisonFunc
            )
        where TConditionParams : ConditionParams
        {
            var conditionType = conditionMetadata.ConditionType;
            ThrowIfConfigured(conditionType);

            var comparisonFuncGeneric = GetGenericComparisonFunc(comparisonFunc);
            conditionMetadata.ParamType = typeof(TConditionParams).Name;
            var condition = new Condition<TCondition, TObject, TProperty>(valueProvider, comparisonFuncGeneric, conditionMetadata);
            _conditions.Add(conditionType, condition);
            MetaData.Add(conditionType, conditionMetadata);
        }

        #endregion

        #region (Helpers)

        private bool ConditionIsConfigured(TCondition _) => _conditions.ContainsKey(_);

        private void ThrowIfConfigured(TCondition _)
        {
            if (!ConditionIsConfigured(_)) return;

            throw new InvalidEnumArgumentException($"Condition {_} is already configured.");
        }

        private void ThrowIfNotConfigured(TCondition _)
        {
            if (ConditionIsConfigured(_)) return;

            throw new InvalidEnumArgumentException($"Condition {_} is not configured.");
        }

        private static Func<TProperty, ConditionParams, bool> GetGenericComparisonFunc<TProperty, TConditionParams>(
            Func<TProperty, TConditionParams, bool> function)
            where TConditionParams : ConditionParams
            => (statType, comparisonParams) => function(statType, comparisonParams as TConditionParams);

        #endregion
    }
}
