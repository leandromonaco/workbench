using CommandLine;
using DependencyTester;
using System;
using System.Security.Principal;

string username = string.Empty;
string password = string.Empty;
string domain = string.Empty;
string hostName = string.Empty;
string ports = string.Empty;
string databaseConnectionString = string.Empty;
string databaseEngine = string.Empty;
bool certificateCheck = false;
string folder = string.Empty;
string seqUrl = string.Empty;
string seqApiKey = string.Empty;

string restUrl = string.Empty;
string restVerb = string.Empty;
string restUser = string.Empty;
string restPassword = string.Empty;
string proxyUri = string.Empty;
string proxyUser = string.Empty;
string proxyPassword = string.Empty;
bool ignoreSslErrors = false;

string smtpHost = string.Empty;
int smtpPort = 25;
string emailRecipient = string.Empty;


Parser.Default.ParseArguments<Options>(args)
       .WithParsed(o =>
       {
           username = o.Username;
           password = o.Password;
           domain = o.Domain;
           hostName = o.HostName;
           ports = o.Ports;
           databaseConnectionString = o.DatabaseConnectionString;
           databaseEngine = o.DatabaseEngine;
           certificateCheck = o.CertificateCheck;
           folder = o.Folder;
           seqUrl = o.SeqUrl;
           seqApiKey = o.SeqApiKey;
           restUrl = o.RestUrl;
           restVerb = o.RestVerb;
           restUser = o.RestUser;
           restPassword = o.RestPassword;
           proxyUri = o.ProxyUri;
           proxyUser = o.ProxyUser;
           proxyPassword = o.ProxyPassword;
           smtpHost = o.SmtpHost;
           smtpPort = o.SmtpPort;
           emailRecipient = o.EmailRecipient;
           ignoreSslErrors = o.ignoreSslErrors;
       });

if (!string.IsNullOrEmpty(smtpHost) && !string.IsNullOrEmpty(emailRecipient))
{
    Helper.SendTestEmail(smtpHost, smtpPort, emailRecipient);
}


if (!string.IsNullOrEmpty(restUrl) && !string.IsNullOrEmpty(restVerb))
{
    Helper.CheckRestService(restUrl, restVerb, restUser, restPassword, proxyUri, proxyUser, proxyPassword, ignoreSslErrors);
}

if (!string.IsNullOrEmpty(seqUrl) && !string.IsNullOrEmpty(seqApiKey))
{
    Helper.CheckSeqLogging(seqUrl, seqApiKey);
}

if (!string.IsNullOrEmpty(folder))
{
    if (!string.IsNullOrEmpty(username) &&
        !string.IsNullOrEmpty(password) &&
        !string.IsNullOrEmpty(domain))
    {
        var impersonationToken = Helper.GetImpersonationToken(username, password, domain);

        WindowsIdentity.RunImpersonated(impersonationToken, () =>
        {
            Console.WriteLine($"RUN AS: {WindowsIdentity.GetCurrent().Name}");
            Helper.CheckFolderAccess(folder);
        });
    }
    else
    {
        Helper.CheckFolderAccess(folder);
    }
    
}

if (!string.IsNullOrEmpty(hostName))
{
    Helper.CheckServer(hostName);
    if (!string.IsNullOrEmpty(ports))
    {
        var portsSplit = ports.Split(',');
        foreach (var port in portsSplit)
        {
            Helper.CheckPort(hostName, int.Parse(port));
        }
    }
    if (certificateCheck)
    {
        Helper.CheckCertificate($"https://{hostName}");
    }
}

if (!string.IsNullOrEmpty(databaseConnectionString))
{
    if (!string.IsNullOrEmpty(databaseEngine))
    {
        switch (databaseEngine)
        {
            case "Oracle":
                Helper.CheckOracleDatabase(databaseConnectionString);
                break;
            case "SqlServer":
                Helper.CheckSqlServerDatabase(databaseConnectionString);
                break;
            default:
                break;
        }
    }
}
   
Console.ResetColor();
Console.WriteLine("Process has finished. Use --help to see the options.");