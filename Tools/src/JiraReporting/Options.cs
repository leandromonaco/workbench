using CommandLine;

namespace JiraReporting
{
    public class Options
    {
        [Option("JiraEndpoint", Required = true, HelpText = "JIRA Endpoint")]
        public string JiraEndpoint { get; set; }

        [Option("JiraProject", Required = true, HelpText = "JIRA Project")]
        public string JiraProject { get; set; }

        [Option("JiraUsername", Required = true, HelpText = "JIRA Username")]
        public string JiraUsername { get; set; }

        [Option("JiraAuthenticationToken", Required = true, HelpText = "JIRA Authentication Token")]
        public string JiraAuthenticationToken { get; set; }

        [Option("PowerBiDatasetEndpoint", Required = true, HelpText = "PowerBI Endpoint")]
        public string PowerBiDatasetEndpoint { get; set; }

    }
}