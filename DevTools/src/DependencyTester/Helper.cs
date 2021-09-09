using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Permissions;
using System.Security;
using Oracle.ManagedDataAccess.Client;
using Serilog;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using Serilog.Debugging;
using System.Text;
using System.Net.Security;

namespace DependencyTester
{
    class Helper
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool LogonUser(String Username, String Domain, String Password, int LogonType, int LogonProvider, out SafeAccessTokenHandle Token);

        const int LOGON32_PROVIDER_DEFAULT = 0;
        //This parameter causes LogonUser to create a primary token.     
        const int LOGON32_LOGON_INTERACTIVE = 2;

        public static SafeAccessTokenHandle GetImpersonationToken(string username, string password, string domain)
        {
            // Call LogonUser to obtain a handle to an access token.     
            LogonUser(username, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out SafeAccessTokenHandle safeAccessTokenHandle);
            return safeAccessTokenHandle;
        }

        public static void CheckFolderAccess(string folder)
        {
            var testFile = $"{folder}\\test.txt";
            var permissionSet = new PermissionSet(PermissionState.None);
            var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, folder);
            permissionSet.AddPermission(writePermission);

            try
            {
                using (FileStream fstream = new FileStream(testFile, FileMode.Create))
                using (TextWriter writer = new StreamWriter(fstream))
                {
                    writer.WriteLine("This is a test");
                    Console.ForegroundColor = System.ConsoleColor.Green;
                    Console.WriteLine($"FOLDER: {folder} OK");
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = System.ConsoleColor.Red;
                Console.WriteLine($"FOLDER: {folder} Failed");
            }

        }

        internal static void CheckRestService(string restUrl, string restVerb, string restUser, string restPassword, string proxy, bool ignoreSslErrors)
        {
            HttpResponseMessage result = null;

            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                                                                        {

                                                                            var certificate = new X509Certificate2(cert.GetRawCertData());
                                                                            Console.WriteLine($"sslPolicyErrors {sslPolicyErrors.ToString()} | Subject {cert.Subject} | NotAfter {certificate.NotAfter} | NotBefore {certificate.NotBefore} | Issuer {certificate.Issuer}");
                                                                            if (ignoreSslErrors)
                                                                            {
                                                                                return true;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (sslPolicyErrors == SslPolicyErrors.None)
                                                                                {
                                                                                    return true;   //Is valid
                                                                            }
                                                                                return false;
                                                                            }
                                                                        }
            };

            
            if (!string.IsNullOrEmpty(proxy))
            {
                httpClientHandler.Proxy = new WebProxy(proxy);
            }

            var httpClient = new HttpClient(httpClientHandler);

            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{restUser}:{restPassword}"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {key}");

            if (restVerb.Equals("GET"))
            {
                result = httpClient.GetAsync(restUrl).Result;
            }

            if (result != null && result.StatusCode.Equals(HttpStatusCode.OK))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"REST Service: {restUrl} OK");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"REST Service: {restUrl} Failed with Status Code {result.StatusCode} ({result.ReasonPhrase})");
            }
            Console.WriteLine($"REST Response: {result.Content.ReadAsStringAsync().Result}");
        }

        public static void CheckSeqLogging(string seqLoggingUrl, string seqLoggingApiKey)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                                            .WriteTo.Seq(serverUrl: seqLoggingUrl, apiKey: seqLoggingApiKey).CreateLogger();
                SelfLog.Enable(m => throw new Exception());
                Log.Information("This is a connectivity test");
                Log.CloseAndFlush();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{seqLoggingUrl} Connection OK");
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{seqLoggingUrl} Connection Failed");
            }
        }

        public static void CheckOracleDatabase(string connectionString)
        {
            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                conn.Open();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{connectionString} Connection OK");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{connectionString} Connection Failed");
            }
        }

        public static void CheckSqlServerDatabase(string connectionString)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{connectionString} Connection OK");
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{connectionString} Connection Failed");
            }
        }

        public static void CheckServer(string serverName)
        {
            CheckDns(serverName);
            PingServer(serverName);
        }

        public static void CheckDns(string dnsEntry)
        {
            IPHostEntry hostEntry = null;
            string ipAddress = string.Empty;
            try
            {
                hostEntry = Dns.GetHostEntry(dnsEntry);
                ipAddress = hostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
            }
            catch (Exception)
            {

            }

            if (hostEntry != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"DNS: {dnsEntry} ({ipAddress}) OK");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"DNS: {dnsEntry} Failed");
            }
        }

        public static void CheckPort(string serverName, int portNumber)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 |
                                       SecurityProtocolType.Tls11 |
                                       SecurityProtocolType.Tls13;

            using TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(serverName, portNumber);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"PORT: {serverName} {portNumber} OK");
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"PORT: {serverName} {portNumber} Failed");
            }
        }

        public static void PingServer(string hostName)
        {
            var pinger = new Ping();
            PingReply reply = pinger.Send(hostName);
            var pingable = reply.Status == IPStatus.Success;
            if (pingable)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"PING: {hostName} OK");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"PING: {hostName} Failed");
            }
        }

        public static void CheckCertificate(string hostName)
        {
            X509Certificate2 certificate = null;
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, error) =>
                {
                    certificate = new X509Certificate2(cert.GetRawCertData());
                    return true;
                }
            };

            var httpClient = new HttpClient(httpClientHandler);
            httpClient.Send(new HttpRequestMessage(HttpMethod.Head, hostName));

            var verb = "expires";

            if (certificate.NotAfter < DateTime.Now)
            {
                //Certificate is expired
                Console.ForegroundColor = ConsoleColor.Red;
                verb = "expired";
            }
            else if (certificate.NotAfter < DateTime.Now.AddDays(60))
            {
                //Certificate is about to expire
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                //Certificate is valid
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"{hostName} certificate {verb} on {certificate.NotAfter}");

        }
    }
}
