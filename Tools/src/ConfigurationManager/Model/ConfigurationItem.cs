using System.Collections.Generic;

namespace ConfigurationManager.Model
{
    public class ConfigurationItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<EnvironmentVariable> EnvironmentVariables { get; set; }
    }
}