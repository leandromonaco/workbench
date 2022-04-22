using System.Security.Cryptography.X509Certificates;

namespace TeamHub.Core
{
    public static class Security
    {
        public static X509Certificate2 LoadCertificate()
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.MaxAllowed);
            X509Certificate2 cert = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(c => c.Subject == "CN=localhost");
            store.Close();
            return cert;
        }
    }
}