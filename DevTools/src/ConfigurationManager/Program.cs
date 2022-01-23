using CommandLine;
using ConfigurationManager.Model;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ConfigurationManager
{
    class Program
    {
        static void Main(string[] args)
        {

            string configuration = string.Empty;
            string application = string.Empty;
            string environment = string.Empty;

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       configuration = o.ConfigurationFile;
                       application = o.ApplicationName;
                       environment = o.DeploymentEnvironment;
                   });


            var sourceConfigurationFileInfo = new FileInfo(configuration);

            string configurationFileContent = File.ReadAllText(configuration);

            string deploymentVariablesFileContent = File.ReadAllText(@$"{Directory.GetCurrentDirectory()}\Variables\{application}.variables.json");

            DeploymentConfiguration config = JsonSerializer.Deserialize<DeploymentConfiguration>(deploymentVariablesFileContent);

            var environmentVariables = config.Environments.Where(e => e.Name.Equals(environment)).FirstOrDefault();

            foreach (var variable in environmentVariables.Variables)
            {
                if (!variable.IsSecret)
                {
                    configurationFileContent = configurationFileContent.Replace($"#{{{variable.Name}}}", variable.Value);
                }
                else
                {
                    //Get value from vault
                }
            }

            File.Delete(configuration);
            var outputDirectory = sourceConfigurationFileInfo.Directory;
            var outputFilename = sourceConfigurationFileInfo.Name.Replace(".Release","");
            File.WriteAllText(@$"{outputDirectory}\{outputFilename}", configurationFileContent);
        }
    }
}
