using CommandLine;

namespace JiraReporting
{
    public class Options
    {
        [Option("Endpoint", Required = true, HelpText = "Endpoint Filter")]
        public string Endpoint { get; set; }
    }
}