using Azure.Identity;
using Azure.Security.KeyVault.Keys;
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

            foreach (var variable in config.Variables)
            {
                if (!variable.IsSecret)
                {
                    configurationFileContent = configurationFileContent.Replace($"#{{{variable.Name}}}", variable.Values.FirstOrDefault(e => e.Name.Equals(environment)).Value);
                }
                else
                {
                    //Get value from vault
                    //https://github.com/Azure/azure-sdk-for-net/blob/Azure.Security.KeyVault.Keys_4.2.0/sdk/keyvault/Azure.Security.KeyVault.Keys/README.md
                    var client = new KeyClient(vaultUri: new Uri("https://configuration-secrets.vault.azure.net/"), credential: new DefaultAzureCredential());
                    // Create a new key using the key client.
                    KeyVaultKey key = client.CreateKey("key-name", KeyType.Rsa);

                    // Retrieve a key using the key client.
                    key = client.GetKey("key-name");
                }
            }

            File.Delete(configuration);
            var outputDirectory = sourceConfigurationFileInfo.Directory;
            var outputFilename = sourceConfigurationFileInfo.Name.Replace(".Release","");
            File.WriteAllText(@$"{outputDirectory}\{outputFilename}", configurationFileContent);
        }
    }
}
