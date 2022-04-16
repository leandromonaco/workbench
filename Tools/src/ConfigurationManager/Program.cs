using CommandLine;
using ConfigurationManager.Model;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ConfigurationManager
{
    class Program
    {
        static void Main(string[] args)
        {

            string applicationConfiguration = string.Empty;
            string application = string.Empty;
            string environment = string.Empty;
            string mode = string.Empty;

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       applicationConfiguration = o.ConfigurationFile;
                       application = o.ApplicationName;
                       environment = o.Environment;
                       mode = o.Mode;
                   });

            string deploymentVariablesFileContent = File.ReadAllText(@$"{Directory.GetCurrentDirectory()}\Variables\{application}.variables.json");
            DeploymentConfiguration deploymentConfiguration = JsonSerializer.Deserialize<DeploymentConfiguration>(deploymentVariablesFileContent);

            switch (mode)
            {
                case "Save":
                    SaveDeploymentConfiguration(deploymentConfiguration);
                    break;
                case "Transform":
                    TransformConfigurationFile(applicationConfiguration, environment, deploymentConfiguration);
                    break;
            }
        }

        private static void SaveDeploymentConfiguration(DeploymentConfiguration config)
        {
            //Save Mode
            foreach (var configurationItem in config.ConfigurationItems)
            {
                foreach (var environmentVariable in configurationItem.EnvironmentVariables)
                {
                    if (environmentVariable.IsSecret)
                    {
                        if (!environmentVariable.IsEncrypted)
                        {
                            environmentVariable.Value = ""; //encrypt logic
                            environmentVariable.IsEncrypted = true;
                        }
                    }
                }
            }

            //Serialize and save DeploymentConfiguration

            UpdateVariableDocumentation(config);
        }

        private static void UpdateVariableDocumentation(DeploymentConfiguration config)
        {
            var markDownContent = new StringBuilder();

            var sortedVariables = config.ConfigurationItems.OrderBy(v => v.Name).ToList();

            // Build Index
            markDownContent.AppendLine("# Index");
            foreach (var variable in sortedVariables)
            {
                markDownContent.AppendLine($"- [{variable.Name}](#{variable.Name.ToLower()})");
            }
            markDownContent.AppendLine();

            // Build Documentation

            foreach (var variable in sortedVariables)
            {
                markDownContent.AppendLine($"## {variable.Name}");
                markDownContent.AppendLine();
                markDownContent.AppendLine($"**Description**: {variable.Description}");
                markDownContent.AppendLine();

                markDownContent.AppendLine("|Environment|Value|");
                markDownContent.AppendLine("|-|-|");
                foreach (var environmentVariable in variable.EnvironmentVariables)
                {
                    if (!environmentVariable.IsSecret)
                    {
                        markDownContent.AppendLine($"|{environmentVariable.Name}|{environmentVariable.Value}|");
                    }
                    else
                    {
                        markDownContent.AppendLine("value is stored in the vault.");
                    }
                }

                markDownContent.AppendLine();
            }
        }

        private static string TransformConfigurationFile(string configuration, string environment, DeploymentConfiguration config)
        {
            var sourceConfigurationFileInfo = new FileInfo(configuration);
            string configurationFileContent = File.ReadAllText(configuration);

            //Transform Mode
            foreach (var variable in config.ConfigurationItems)
            {
                var environmentVariable = variable.EnvironmentVariables.FirstOrDefault(e => e.Name.Equals(environment));

                if (!environmentVariable.IsSecret)
                {
                    configurationFileContent = configurationFileContent.Replace($"#{{{variable.Name}}}", environmentVariable.Value);
                }
                else
                {
                    //Decrypt environmentVariable.Value
                }
            }

            File.Delete(configuration);
            var outputDirectory = sourceConfigurationFileInfo.Directory;
            var outputFilename = sourceConfigurationFileInfo.Name.Replace(".Release", "");
            File.WriteAllText(@$"{outputDirectory}\{outputFilename}", configurationFileContent);
            return configurationFileContent;
        }
    }
}
