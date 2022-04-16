using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CodeKata.CSharp.Helpers.Kata2_EncryptionUsingCertificate
{
    internal class Kata2_Helper
    {
        public static X509Certificate2 LoadCertificate()
        {
            X509Store store = new X509Store(StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection =
               store.Certificates;
            X509Certificate2 cert =
               certCollection.Cast<X509Certificate2>().FirstOrDefault
               (c => c.Subject == "CN=ConfigurationManager");
            if (cert == null)
                Console.WriteLine("NO Certificate named ConfigurationManager was found in your certificate store");
            store.Close();
            return cert;
        }

        public static string Encrypt(X509Certificate2 x509, string stringToEncrypt)
        {
            if (x509 == null || string.IsNullOrEmpty(stringToEncrypt))
                throw new Exception("A x509 certificate and string for encryption must be provided");

            using (var rsa = x509.GetRSAPublicKey())
            {
                var text = Encoding.UTF8.GetBytes(stringToEncrypt);
                var result = rsa.Encrypt(text, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(result);
            }
        }

 
        public static string Decrypt(X509Certificate2 x509, string stringTodecrypt)
        {
            if (x509 == null || string.IsNullOrEmpty(stringTodecrypt))
                throw new Exception("A x509 certificate and string for decryption must be provided");

            if (!x509.HasPrivateKey)
                throw new Exception("x509 certicate does not contain a private key for decryption");

            using (var rsa = x509.GetRSAPrivateKey())
            {
                byte[] bytestodecrypt = Convert.FromBase64String(stringTodecrypt);
                byte[] plainbytes = rsa.Decrypt(bytestodecrypt, RSAEncryptionPadding.Pkcs1);
                ASCIIEncoding enc = new ASCIIEncoding();
                return enc.GetString(plainbytes);
            }
        }
    }
}
