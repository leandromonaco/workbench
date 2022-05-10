using System.Text;
using CommandLine;
using ConfluenceKB;
using IntegrationConnectors.Common;
using IntegrationConnectors.Confluence;


Parser.Default.ParseArguments<Options>(args)
              .WithParsed(o =>
              {
                   ExecuteJob(o.ConfluenceEndpoint, o.ConfluenceUsername, o.ConfluenceAuthenticationToken, o.UserAccountId, o.SpaceKey, o.Label, o.LastModifiedYear, o.CreatedYear);
               });

void ExecuteJob(string confluenceEndpoint, string confluenceUsername, string confluenceAuthenticationToken, string userAccountId, string spaceKey, string label, string lastModifiedYear, string createdYear)
{
    var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{confluenceUsername}:{confluenceAuthenticationToken}"));
    var confluenceConnector = new ConfluenceConnector(confluenceEndpoint, key, AuthenticationType.Basic);
    var results = confluenceConnector.SearchContentAsync(userAccountId, spaceKey, label, lastModifiedYear, createdYear).Result;
    foreach (var result in results)
    {
        Console.WriteLine($"{confluenceEndpoint}/wiki{result.Links.WebUi}");
    }
}

