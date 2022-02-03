using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OctopusBackup.Model
{
    public class OctopusProjectBackup
    {
        public JsonDocument Process { get; set; }
        public OctopusVariableBackup Variables { get; set; }
    }
}
