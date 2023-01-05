using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiraReporting.Model;

namespace IntegrationConnectors.JIRA.Model
{
    internal class JiraSprintsResult
    {
        public bool IsLast { get; set; }
        public List<JiraSprint> Values { get; set; }
    }
}
