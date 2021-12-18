using BoxFilterExample;
using ConfigurableValidationFilter;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BoxFilterPOC.Tests
{
    public class BoxFilterSerializationTests
    {
        [Test]
        public void SerializationWorksWhenConfigured()
        {
            var originalConfig = BoxFilterConfigurations.PlainBrownBox;

            var serialized = JsonConvert.SerializeObject(originalConfig);
            var deserialized = JsonConvert.DeserializeObject<FilterConfiguration<BoxRules>>(serialized);

            Assert.That(deserialized != null);

            deserialized.Groups.Count.Should().Be(originalConfig.Groups.Count);

            for (var i = 0; i < originalConfig.Groups.Count; i++)
            {
                var originalGroupConfig = originalConfig.Groups[i];
                var deserializedGroupConfig = deserialized.Groups[i];

                originalGroupConfig.Conditions.Count.Should().Be(deserializedGroupConfig.Conditions.Count);

                for (var j = 0; j < originalGroupConfig.Conditions.Count; j++)
                {
                    var originalConditionConfig = originalGroupConfig.Conditions[j];
                    var deserializedConditionConfig = deserializedGroupConfig.Conditions[j];

                    originalConditionConfig.Type.Should().Be(deserializedConditionConfig.Type);
                    originalConditionConfig.Params.GetType().FullName.Should().Be(deserializedConditionConfig.Params.GetType().FullName);
                }
            }
        }

        [Test]
        public void Save()
        {
            var storage = new LocalStorage<FilterConfiguration<BoxRules>>("BoxFilterConfigurations");

            storage.Save(nameof(BoxFilterConfigurations.IsWarmColor), BoxFilterConfigurations.IsWarmColor);

            storage.Load(nameof(BoxFilterConfigurations.IsWarmColor));
        }
    }
}
