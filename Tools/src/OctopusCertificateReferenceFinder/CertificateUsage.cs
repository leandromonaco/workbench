using System;

namespace SearchInFiles
{
    public class CertificateUsage
    {
        public string Project { get; set; }
        public string CertificateName { get; set; }
        public string Thumbprint { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}