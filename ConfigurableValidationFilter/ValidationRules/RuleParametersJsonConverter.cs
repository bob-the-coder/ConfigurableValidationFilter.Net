﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConfigurableValidationFilter.Prefabs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SystemJsonSerializer = System.Text.Json.JsonSerializer;
using NewtonsoftJsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace ConfigurableValidationFilter.ValidationRules
{
    public class RuleParametersJsonConverter : JsonConverter<RuleParameters>
    {
        private const string ParamTypeDiscriminator = "__param_type__";

        protected static readonly Dictionary<Type, string> ParamTypeMap = new();

        private static bool IsParamTypeIncluded(Type paramType)
            => ParamTypeMap.ContainsKey(paramType);

        private static bool IsParamTypeIncluded(string paramTypeName)
            => ParamTypeMap.ContainsValue(paramTypeName);

        static RuleParametersJsonConverter()
        {
            Include<RuleParameters>();
            Include<IntValueParam>();
            Include<StringValueParam>();
            Include<DateTimeValueParam>();
            Include<BoolValueParam>();
            Include<IntListParam>();
            Include<StringListParam>();
            Include<IntMinMaxParams>();
            Include<DoubleMinMaxParams>();
            Include<DateTimeMinMaxParams>();
        }

        public static void Include<TConditionParams>()
            where TConditionParams : RuleParameters
        {
            var paramType = typeof(TConditionParams);
            if (IsParamTypeIncluded(paramType)) throw new ArgumentException("Param types must specified only once.");

            ParamTypeMap.Add(paramType, paramType.Name);
        }

        private static void ThrowInvalidConfigurationException(string paramTypeName)
        {
            throw new ArgumentException($"Invalid filter configuration. Param type {paramTypeName} is not supported.");
        }

        public override void WriteJson(JsonWriter writer, RuleParameters value, NewtonsoftJsonSerializer serializer)
        {
            var paramType = value.GetType();
            if (!IsParamTypeIncluded(paramType)) ThrowInvalidConfigurationException(paramType.Name);

            var jo = JObject.Parse(SystemJsonSerializer.Serialize(value, paramType));

            serializer.Populate(jo.CreateReader(), value);
            jo.Add(ParamTypeDiscriminator, ParamTypeMap[paramType]);

            jo.WriteTo(writer);
        }

        public override RuleParameters ReadJson(JsonReader reader, Type objectType, RuleParameters existingValue, bool hasExistingValue,
            NewtonsoftJsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var paramTypeName = (string)jo[ParamTypeDiscriminator];
            if (!IsParamTypeIncluded(paramTypeName)) ThrowInvalidConfigurationException(paramTypeName);

            var paramType = ParamTypeMap.First(kv => kv.Value == paramTypeName).Key;
            var conditionParamsObject = Activator.CreateInstance(paramType);
            if (conditionParamsObject is not RuleParameters conditionParams) return null;

            serializer.Populate(jo.CreateReader(), conditionParams);

            return conditionParams;
        }

    }
}
