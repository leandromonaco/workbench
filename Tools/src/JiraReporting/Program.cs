using CommandLine;
using Hangfire;
using Hangfire.MemoryStorage;
using IntegrationConnectors.Common;
using IntegrationConnectors.JIRA;
using IntegrationConnectors.JIRA.Model;
using JiraReporting.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace JiraReporting
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseMemoryStorage();

            Parser.Default.ParseArguments<Options>(args)
               .WithParsed(o =>
               {
                   ExecuteJob(o.JiraEndpoint, o.JQL, o.JiraUsername, o.JiraAuthenticationToken);
               });

            using var server = new BackgroundJobServer();
            Console.ReadLine();
        }

        public static void ExecuteJob(string jiraEndpoint, string jql, string jiraUsername, string jiraAuthenticationToken)
        {
            Console.WriteLine($"Jira Report started {DateTime.Now}");

            var dateFormat = "yyyy-MM-dd";

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{jiraUsername}:{jiraAuthenticationToken}"));

            var jiraConnector = new JiraConnector(jiraEndpoint, credentials, AuthenticationType.Basic);

            List<JiraBacklogItem> latestBacklog = jiraConnector.GetBacklogItemsAsync(jql).Result;
            var outputFile = $"{Environment.CurrentDirectory}\\report_{DateTime.Now.Date.ToString(dateFormat)}";
            File.WriteAllText($"{outputFile}.json", JsonSerializer.Serialize(latestBacklog));
            ExcelHelper.Export(latestBacklog, null, $"{outputFile}.xlsx", jiraEndpoint);

            Console.WriteLine($"Jira Report finished {DateTime.Now}");
        }
    }
}
