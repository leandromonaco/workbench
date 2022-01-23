using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationManager.Model
{
    public class Environment
    {
        public string Name { get; set; }
        public List<Variable> Variables { get; set; }
    }
}
