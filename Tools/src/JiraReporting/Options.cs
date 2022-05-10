using CommandLine;

namespace JiraReporting
{
    public class Options
    {
        [Option("JiraEndpoint", Required = true, HelpText = "JIRA Endpoint")]
        public string JiraEndpoint { get; set; }

        [Option("JQL", Required = true, HelpText = "JIRA Query Language (https://support.atlassian.com/jira-software-cloud/docs/use-advanced-search-with-jira-query-language-jql/)")]
        public string JQL { get; set; }

        [Option("JiraUsername", Required = true, HelpText = "JIRA Username")]
        public string JiraUsername { get; set; }

        [Option("JiraAuthenticationToken", Required = true, HelpText = "JIRA Authentication Token (https://id.atlassian.com/manage-profile/security/api-tokens)")]
        public string JiraAuthenticationToken { get; set; }
    }
}