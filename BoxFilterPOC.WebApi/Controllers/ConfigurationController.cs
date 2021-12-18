using System;
using System.Collections.Generic;
using BoxFilterExample;
using ConfigurableValidationFilter;
using ConfigurableValidationFilter.ValidationRules;
using Microsoft.AspNetCore.Mvc;

namespace BoxFilterPOC.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly BoxFilter _filter = BoxFilter.Default();
        private readonly LocalStorage<List<string>> _lists = new("Stores");
        private readonly LocalStorage<FilterConfiguration<BoxRules>> _configurations = new("BoxFilterConfigurations");

        private static Dictionary<BoxRules, RuleMetadata<BoxRules>> _metadata;

        public ConfigurationController()
        {
            CreateListIfNull();
        }

        [HttpGet]
        [Route("api/{controller}/metadata")]
        public Dictionary<BoxRules, RuleMetadata<BoxRules>> Metadata()
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
        public FilterConfiguration<BoxRules> Get(string id)
        {
            return _configurations.Load(id);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _configurations.Delete(id);
        }

        [HttpPost]
        public FilterConfiguration<BoxRules> Configuration([FromBody] FilterConfiguration<BoxRules> configuration)
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
