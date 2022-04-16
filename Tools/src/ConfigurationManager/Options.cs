using CommandLine;

namespace ConfigurationManager
{
    public class Options
    {
        [Option("configuration", Required = false, HelpText = "Configuration File for Release")]
        public string ConfigurationFile { get; set; }

        [Option("application", Required = false, HelpText = "Application Name")]
        public string ApplicationName { get; set; }

        [Option("environment", Required = false, HelpText = "Deployment Environment")]
        public string Environment { get; set; }

        [Option("mode", Required = false, HelpText = "Modes: Save / Transform")]
        public string Mode { get; set; }
    }
}