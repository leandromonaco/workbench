using CommandLine;
using JiraReporting;
using System;
using System.Net;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;

namespace NetworkCapture
{
    class Program
    {
        static string _endpointFilter;

        static void Main(string[] args)
        {

            Parser.Default.ParseArguments<Options>(args)
              .WithParsed(o =>
              {
                  _endpointFilter = o.Endpoint;
               });


            var proxyServer = new ProxyServer();

            // locally trust root certificate used by this proxy 
            //proxyServer.CertificateManager.TrustRootCertificate(true);

            // optionally set the Certificate Engine
            // Under Mono only BouncyCastle will be supported
            //proxyServer.CertificateManager.CertificateEngine = Network.CertificateEngine.BouncyCastle;

            proxyServer.BeforeRequest += OnRequest;
            proxyServer.BeforeResponse += OnResponse;
            proxyServer.ServerCertificateValidationCallback += OnCertificateValidation;
            proxyServer.ClientCertificateSelectionCallback += OnCertificateSelection;


            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true)
            {
                // Use self-issued generic certificate on all https requests
                // Optimizes performance by not creating a certificate for each https-enabled domain
                // Useful when certificate trust is not required by proxy clients
                //GenericCertificate = new X509Certificate2(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "genericcert.pfx"), "password")
            };

            // Fired when a CONNECT request is received
            //explicitEndPoint.BeforeTunnelConnectRequest += OnBeforeTunnelConnectRequest;

            // An explicit endpoint is where the client knows about the existence of a proxy
            // So client sends request in a proxy friendly manner
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();

            // Transparent endpoint is useful for reverse proxy (client is not aware of the existence of proxy)
            // A transparent endpoint usually requires a network router port forwarding HTTP(S) packets or DNS
            // to send data to this endPoint
            //var transparentEndPoint = new TransparentProxyEndPoint(IPAddress.Any, 8001, true)
            //{
            //    // Generic Certificate hostname to use
            //    // when SNI is disabled by client
            //    GenericCertificateName = "google.com"
            //};

            //proxyServer.AddEndPoint(transparentEndPoint);

            //proxyServer.UpStreamHttpProxy = new ExternalProxy() { HostName = "localhost", Port = 8888 };
            //proxyServer.UpStreamHttpsProxy = new ExternalProxy() { HostName = "localhost", Port = 8888 };

            foreach (var endPoint in proxyServer.ProxyEndPoints)
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ",
                    endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);

            // Only explicit proxies can be set as system proxy!
            proxyServer.SetAsSystemHttpProxy(explicitEndPoint);
            proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);

            // wait here (You can use something else as a wait function, I am using this as a demo)
            Console.Read();

            // Unsubscribe & Quit
            //explicitEndPoint.BeforeTunnelConnectRequest -= OnBeforeTunnelConnectRequest;
            proxyServer.BeforeRequest -= OnRequest;
            proxyServer.BeforeResponse -= OnResponse;
            proxyServer.ServerCertificateValidationCallback -= OnCertificateValidation;
            proxyServer.ClientCertificateSelectionCallback -= OnCertificateSelection;

            proxyServer.Stop();

        }

        //private static async Task OnBeforeTunnelConnectRequest(object sender, TunnelConnectSessionEventArgs e)
        //{
        //    string hostname = e.HttpClient.Request.RequestUri.Host;

        //    if (hostname.Contains("dropbox.com"))
        //    {
        //        // Exclude Https addresses you don't want to proxy
        //        // Useful for clients that use certificate pinning
        //        // for example dropbox.com
        //        e.DecryptSsl = false;
        //    }
        //}

        public static async Task OnRequest(object sender, SessionEventArgs e)
        {
            // read request headers
            var requestHeaders = e.HttpClient.Request.Headers;

            var method = e.HttpClient.Request.Method.ToUpper();

            // Get/Set request body bytes
            byte[] bodyBytes = await e.GetRequestBody();
            e.SetRequestBody(bodyBytes);

            // Get/Set request body as string
            string bodyString = await e.GetRequestBodyAsString();
            e.SetRequestBodyString(bodyString);

            //if (e.HttpClient.Request.Url.Contains(_endpointFilter))
            //{
            Console.WriteLine($"Request URL: {e.HttpClient.Request.Url}");
            Console.WriteLine($"Request Method: {method}");
            Console.WriteLine($"Request Body: {bodyString}");
            //}


            // store request 
            // so that you can find it from response handler 
            e.UserData = e.HttpClient.Request;
        }

        // Modify response
        public static async Task OnResponse(object sender, SessionEventArgs e)
        {
            if (e.HttpClient.Request.Url.Contains(_endpointFilter) &&
                e.HttpClient.Response.ContentType != null &&
                e.HttpClient.Response.ContentType.Contains("application/json"))
            {
                byte[] bodyBytes = await e.GetResponseBody();
                e.SetResponseBody(bodyBytes);

                string bodyString = await e.GetResponseBodyAsString();
                e.SetResponseBodyString(bodyString);

                Console.WriteLine($"Response StatusCode: {e.HttpClient.Response.StatusCode}");
                Console.WriteLine($"Response ContentType: {e.HttpClient.Response.ContentType}");
                Console.WriteLine($"Response Body: {bodyString}");
            }


            if (e.UserData != null)
            {
                // access request from UserData property where we stored it in RequestHandler
                var request = (Request)e.UserData;
            }
        }

        // Allows overriding default certificate validation logic
        public static Task OnCertificateValidation(object sender, CertificateValidationEventArgs e)
        {
            // set IsValid to true/false based on Certificate Errors
            if (e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                e.IsValid = true;

            return Task.CompletedTask;
        }

        // Allows overriding default client certificate selection logic during mutual authentication
        public static Task OnCertificateSelection(object sender, CertificateSelectionEventArgs e)
        {
            // set e.clientCertificate to override
            return Task.CompletedTask;
        }

    }
}
