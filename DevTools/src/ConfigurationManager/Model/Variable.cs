using System.Collections.Generic;

namespace ConfigurationManager.Model
{
    public class Variable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSecret { get; set; }
        public List<Environment> Values { get; set; }
    }
}