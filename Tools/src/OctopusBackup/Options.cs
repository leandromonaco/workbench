﻿using CommandLine;

namespace OctopusBackup
{
    public class Options
    {
        [Option("apiUrl", Required = true, HelpText = "https://octopusserver/api/")]
        public string ApiUrl { get; set; }

        [Option("apiKey", Required = true, HelpText = "API-123")]
        public string ApiKey { get; set; }

        [Option("space", Required = true, HelpText = "Spaces-1")]
        public string Space { get; set; }  

    }
}