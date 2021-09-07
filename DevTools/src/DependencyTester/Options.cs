using CommandLine;

namespace InfrastructureTester
{
    public class Options
    {
        [Option("username", Required = false, HelpText = "username")]
        public string Username { get; set; }

        [Option("password", Required = false, HelpText = "password")]
        public string Password { get; set; }

        [Option("domain", Required = false, HelpText = "domain")]
        public string Domain { get; set; }
        [Option("hostname", Required = false, HelpText = "hostname")]
        public string HostName { get; set; }
        [Option("ports", Required = false, HelpText = "ports")]
        public string Ports { get; set; }
        [Option("database", Required = false, HelpText = "database connection string")]
        public string DatabaseConnectionString { get; set; }
        [Option("engine", Required = false, HelpText = "database engine")]
        public string DatabaseEngine { get; set; }
        [Option("certificateCheck", Required = false, HelpText = "Certificate Check")]
        public bool CertificateCheck { get; set; }
        [Option("folder", Required = false, HelpText = "folder")]
        public string Folder { get; set; }
        [Option("seqUrl", Required = false, HelpText = "seqUrl")]
        public string SeqUrl { get; set; }
        [Option("seqApiKey", Required = false, HelpText = "seqApiKey")]
        public string SeqApiKey { get; set; }
        [Option("restUrl", Required = false, HelpText = "restUrl")]
        public string RestUrl { get; set; }
        [Option("restVerb", Required = false, HelpText = "restVerb")]
        public string RestVerb { get; set; }
        [Option("restUser", Required = false, HelpText = "restUser")]
        public string RestUser { get; set; }
        [Option("restPassword", Required = false, HelpText = "restPassword")]
        public string RestPassword { get; set; }
    }
}