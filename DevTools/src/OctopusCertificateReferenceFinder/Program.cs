using System.Collections.Generic;
using System;
using System.IO;
using IntegrationConnectors.Octopus.Model;
using System.Text.Json;
using System.Linq;
using SearchInFiles;

var searchText = "cert";
var allFiles = Directory.GetFiles(@"C:\GitHub\monorepo\Misc\Octopus", "*_variables.json", SearchOption.AllDirectories);
var certificates = File.ReadAllText(@"C:\GitHub\monorepo\Misc\Octopus\Spaces-1_certificates.json");

var octopusCertificates = JsonSerializer.Deserialize<OctopusCertificates>(certificates);
var certificateUsages = new List<CertificateUsage>();

foreach (string fileName in allFiles)
{
    FileInfo fileInfo = new(fileName);
    var octopusJson = File.ReadAllText(fileName);
    List<OctopusVariable> octopusVariables = JsonSerializer.Deserialize<List<OctopusVariable>>(octopusJson);
    octopusVariables = octopusVariables.Where(v => v.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)).ToList();
    foreach (var variable in octopusVariables)
    {
        var certificate = octopusCertificates.Items.Where(c => c.Id.Equals(variable.Value, StringComparison.InvariantCultureIgnoreCase) ||
                                                               c.Name.Equals(variable.Value, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

        if (certificate!=null)
        {
            certificateUsages.Add(new CertificateUsage() { 
                Project = fileInfo.Name,
                CertificateName = certificate.Name,
                Thumbprint = certificate.Thumbprint,
                ExpirationDate = certificate.NotAfter
            });
        }
    }
}

Helper.ExportExcel(certificateUsages);

Console.WriteLine("Report has been saved.");
Console.Read();