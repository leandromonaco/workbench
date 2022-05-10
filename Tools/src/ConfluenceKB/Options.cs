using CommandLine;

namespace ConfluenceKB
{
    public class Options
    {
        [Option("ConfluenceEndpoint", Required = true, HelpText = "Confluence Endpoint")]
        public string ConfluenceEndpoint { get; set; }

        [Option("ConfluenceUsername", Required = true, HelpText = "Confluence Username")]
        public string ConfluenceUsername { get; set; }

        [Option("ConfluenceAuthenticationToken", Required = true, HelpText = "Confluence Authentication Token (https://id.atlassian.com/manage-profile/security/api-tokens)")]
        public string ConfluenceAuthenticationToken { get; set; }

        [Option("UserAccountId", Required = false, HelpText = "User Account ID")]
        public string UserAccountId { get; set; }

        [Option("SpaceKey", Required = false, HelpText = "Space Key")]
        public string SpaceKey { get; set; }

        [Option("Label", Required = false, HelpText = "Labels")]
        public string Label { get; set; }

        [Option("LastModifiedYear", Required = false, HelpText = "Last Modified Year")]
        public string LastModifiedYear { get; set; }

        [Option("CreatedYear", Required = false, HelpText = "Created Year")]
        public string CreatedYear { get; set; }

    }
}