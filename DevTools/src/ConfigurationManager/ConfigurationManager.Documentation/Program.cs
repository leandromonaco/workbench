using ConfigurationManager.Model;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ConfigurationManager.Documentation
{
    class Program
    {
        static void Main(string[] args)
        {
            string deploymentVariablesFileContent = File.ReadAllText(@$"{Directory.GetCurrentDirectory()}\TestFile\DemoApplication.variables.json");

            DeploymentConfiguration config = JsonSerializer.Deserialize<DeploymentConfiguration>(deploymentVariablesFileContent);

            var markDownContent = new StringBuilder();

            var sortedVariables = config.Variables.OrderBy(v => v.Name).ToList();

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

                if (!variable.IsSecret)
                {
                    markDownContent.AppendLine("|Environment|Value|");
                    markDownContent.AppendLine("|-|-|");
                    foreach (var environment in variable.Values)
                    {
                        markDownContent.AppendLine($"|{environment.Name}|{environment.Value}|");
                    }
                }
                else
                {
                    markDownContent.AppendLine("value is stored in the vault.");
                }
              
                markDownContent.AppendLine();
            }
        }
    }
}
