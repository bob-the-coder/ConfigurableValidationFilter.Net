﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BoxFilterExample;
using ConfigurableValidationFilter;
using ConfigurableValidationFilter.ValidationRules;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BoxFilterPOC.Ui.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly BoxFilter _filter = BoxFilter.Default();
        private readonly LocalStorage<List<string>> _lists = new("Stores");
        private readonly LocalStorage<FilterConfiguration<BoxRules>> _configurations = new("BoxFilterConfigurations");

        public ConfigurationController()
        {
            CreateListIfNull();
        }

        [HttpGet]
        [Route("metadata")]
        public string Metadata()
        {
            return JsonConvert.SerializeObject(_filter.MetaData.Select(kv => (kv.Key, kv.Value)).ToArray());
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<string> List()
        {
            return _lists.Load("Configurations").ToArray();
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            return JsonConvert.SerializeObject(_configurations.Load(id));
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _configurations.Delete(id);
        }

        [HttpPost]
        public string Configuration([FromBody] string configurationJson)
        {
            var configuration = JsonConvert.DeserializeObject<FilterConfiguration<BoxRules>>(configurationJson);
            if (configuration == null) return null;

            var list = _lists.Load("Configurations");
            if (list.Contains(configuration.Name))
            {
                _configurations.Save(configuration.Name, configuration);
                return JsonConvert.SerializeObject(configuration);
            }

            if (string.IsNullOrWhiteSpace(configuration.Name)) configuration.Name = $"FilterConfiguration_{DateTime.UtcNow.Ticks}";

            list.Add(configuration.Name);
            _lists.Save("Configurations", list);
            _configurations.Save(configuration.Name, configuration);
            return JsonConvert.SerializeObject(configuration);
        }

        [HttpPost]
        [Route("test")]
        public ValidationResult Test([FromBody] string testPayloadJson)
        {
            var testPayload = JsonConvert.DeserializeObject<TestPayload>(testPayloadJson);
            if (testPayload == null)
            {
                return new ValidationResult
                {
                    Success = false,
                    Error = "Test Box or Configuration is invalid."
                };
            }

            var watch = Stopwatch.StartNew();
            var result = _filter.ApplyConfiguration(testPayload.Box, testPayload.Configuration);
            watch.Stop();
            result.MetaData = new
            {
                Elapsed = watch.Elapsed.ToString()
            };

            return result;
        }

        private void CreateListIfNull()
        {
            if (_lists.Load("Configurations") != null) return;

            _lists.Save("Configurations", new List<string>());
        }
    }

    public class TestPayload
    {
        public Box Box { get; set; }
        public FilterConfiguration<BoxRules> Configuration { get; set; }
    }
}
