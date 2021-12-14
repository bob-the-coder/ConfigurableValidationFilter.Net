using System;
using System.Collections.Generic;
using System.Linq;
using ConfigurableFilters.Prefabs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SystemJsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace ConfigurableFilters.Condition
{
    public class DefaultConditionParamsJsonConverter : JsonConverter<ConditionParams>
    {
        private const string ParamTypeDiscriminator = "__param_type__";

        protected static readonly Dictionary<Type, string> ParamTypeMap = new();

        private static bool IsParamTypeAllowed(Type paramType)
            => ParamTypeMap.ContainsKey(paramType);

        private static bool IsParamTypeAllowed(string paramTypeName)
            => ParamTypeMap.ContainsValue(paramTypeName);

        static DefaultConditionParamsJsonConverter()
        {
            AllowParamType<ConditionParams>();
            AllowParamType<IntValueParam>();
            AllowParamType<StringValueParam>();
            AllowParamType<DateTimeValueParam>();
            AllowParamType<BoolValueParam>();
            AllowParamType<IntListParam>();
            AllowParamType<StringListParam>();
            AllowParamType<IntMinMaxParams>();
            AllowParamType<DoubleMinMaxParams>();
            AllowParamType<DateTimeMinMaxParams>();
        }

        public static void AllowParamType<TConditionParams>()
            where TConditionParams : ConditionParams
        {
            var paramType = typeof(TConditionParams);
            if (IsParamTypeAllowed(paramType)) throw new ArgumentException("Param types must specified only once.");

            ParamTypeMap.Add(paramType, paramType.Name);
        }

        private static void ThrowInvalidConfigurationException(string paramTypeName)
        {
            throw new ArgumentException($"Invalid filter configuration. Param type {paramTypeName} is not supported.");
        }

        public override void WriteJson(JsonWriter writer, ConditionParams value, JsonSerializer serializer)
        {
            var paramType = value.GetType();
            if (!IsParamTypeAllowed(paramType)) ThrowInvalidConfigurationException(paramType.Name);

            var jo = JObject.Parse(SystemJsonSerializer.Serialize(value, paramType));

            serializer.Populate(jo.CreateReader(), value);
            jo.Add(ParamTypeDiscriminator, ParamTypeMap[paramType]);

            jo.WriteTo(writer);
        }

        public override ConditionParams ReadJson(JsonReader reader, Type objectType, ConditionParams existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var paramTypeName = (string)jo[ParamTypeDiscriminator];
            if (!IsParamTypeAllowed(paramTypeName)) ThrowInvalidConfigurationException(paramTypeName);

            var paramType = ParamTypeMap.First(kv => kv.Value == paramTypeName).Key;
            var conditionParamsObject = Activator.CreateInstance(paramType);
            if (conditionParamsObject is not ConditionParams conditionParams) return null;

            serializer.Populate(jo.CreateReader(), conditionParams);

            return conditionParams;
        }

    }
}
