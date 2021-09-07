using AutomationToolkit.AzDevOps.Model.PullRequest;
using System.Text.RegularExpressions;

namespace PullRequestsMonitor.UI.Model
{
    public class PullRequest : AzDevOpsPullRequest
    {
        private Regex _teamRegex = new Regex("feature.*/");

        public string Team
        {
            get
            {
                if (_teamRegex.Match(SourceRefName).Success)
                {
                    var team = _teamRegex.Match(SourceRefName).Value.Replace("feature/", "");
                    team = team.Split("/")[0];
                    return team;
                }
                else
                {
                    return "Unknown";
                }
            }
        }
    }
}
