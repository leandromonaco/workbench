using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationManager.Model
{
    public class EnvironmentVariable
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsSecret { get; set; }
        public bool IsEncrypted { get; set; }
    }
}
