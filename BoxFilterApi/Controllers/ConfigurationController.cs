using System;
using System.Collections.Generic;
using BoxFilterExample;
using ConfigurableFilters.Condition;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BoxFilterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurableBoxFilter _filter = ConfigurableBoxFilter.Default();
        private readonly LocalStorage<List<string>> _lists = new("Stores");
        private readonly LocalStorage<FilterConfiguration<BoxCondition>> _configurations = new("BoxFilterConfigurations");

        private static Dictionary<BoxCondition, ConditionMetadata> _metadata;

        public ConfigurationController()
        {
            CreateListIfNull();
        }

        [HttpGet]
        [Route("api/{controller}/metadata")]
        public Dictionary<BoxCondition, ConditionMetadata> Metadata()
        {
            if (_metadata != null) return _metadata;

            _metadata = _filter.MetaData;
            return _metadata;
        }

        [HttpGet]
        public IEnumerable<string> List()
        {
            return _lists.Load("Configurations");
        }

        [HttpGet("{id}")]
        public FilterConfiguration<BoxCondition> Get(string id)
        {
            return _configurations.Load(id);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _configurations.Delete(id);
        }

        [HttpPost]
        public FilterConfiguration<BoxCondition> Configuration([FromBody] FilterConfiguration<BoxCondition> configuration)
        {
            var list = _lists.Load("Configurations");
            if (list.Contains(configuration.Name))
            {
                _configurations.Save(configuration.Name, configuration);
                return configuration;
            }

            configuration.Name = $"FilterConfiguration_{DateTime.UtcNow.Ticks}";

            list.Add(configuration.Name);
            _lists.Save("Configurations", list);
            _configurations.Save(configuration.Name, configuration);
            return configuration;
        }

        private void CreateListIfNull()
        {
            if (_lists.Load("Configurations") != null) return;

            _lists.Save("Configurations", new List<string>());
        }
    }
}
