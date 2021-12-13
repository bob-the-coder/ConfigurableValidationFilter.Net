using System.Collections.Generic;

namespace ConfigurableFilters.Condition
{
    public class ValidationResult
    {
        public bool Success { get; set; }
        public string ValidatedValue { get; set; }
        public string Error { get; set; }
        public object MetaData { get; set; }

        public List<ValidationResult> Internal { get; set; } = new();
    }
}
