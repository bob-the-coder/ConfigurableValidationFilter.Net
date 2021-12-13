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

        public IList<ConditionResult> ApplyConfiguration(TObject obj, FilterConfiguration<TCondition> filterConfiguration)
        {
            var errors = new List<string>();
            foreach (var conditionConfig in filterConfiguration.Conditions)
            {
                ThrowIfNotConfigured(conditionConfig.Type);

                var condition = _conditions[conditionConfig.Type];
                var result = condition.Compare(obj, conditionConfig.Params);
                if (!result.Success) errors.Add($@"
{condition.Metadata.Name} {conditionConfig.Params}. Found value {result.ValidatedValue}. 
{result.Error}");
            }

            if (errors.Count == 0) return new List<ConditionResult>(0);

            return errors.Select(error => new ConditionResult
            {
                Error = error
            }).ToList();
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
