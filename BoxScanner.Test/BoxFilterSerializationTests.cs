using BoxFilterExample;
using ConfigurableFilters.Condition;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BoxFilter.Test
{
    public class BoxFilterSerializationTests
    {
        [Test]
        public void SerializationWorksWhenConfigured()
        {
            var codedFilter = BoxFilters.PlainBrownBox;

            var serialized = JsonConvert.SerializeObject(codedFilter);
            var deserialized = JsonConvert.DeserializeObject<FilterConfiguration<BoxCondition>>(serialized);

            Assert.That(deserialized != null);

            deserialized.Conditions.Count.Should().Be(codedFilter.Conditions.Count);

            for (var i = 0; i < codedFilter.Conditions.Count; i++)
            {
                var conditionConfig = codedFilter.Conditions[i];
                var loadedConditionConfig = deserialized.Conditions[i];

                conditionConfig.Type.Should().Be(loadedConditionConfig.Type);
                conditionConfig.Params.GetType().FullName.Should().Be(loadedConditionConfig.Params.GetType().FullName);
            }
        }

        [Test]
        public void Save()
        {
            var storage = new LocalStorage<FilterConfiguration<BoxCondition>>("BoxFilterConfigurations");

            storage.Save(nameof(BoxFilters.IsWarmColor), BoxFilters.IsWarmColor);

            storage.Load(nameof(BoxFilters.IsWarmColor));
        }
    }
}
