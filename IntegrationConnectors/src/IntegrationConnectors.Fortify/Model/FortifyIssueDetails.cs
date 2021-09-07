using System.Collections.Generic;

namespace IntegrationConnectors.Fortify.Model
{
    public class FortifyIssueDetails
    {
        private const string OWASP_VERSION = "OWASP Top 10 2017";
        private readonly List<string> _owaspCategories = new List<string>() {
                                                                        "A1 Injection",
                                                                        "A2 Broken Authentication",
                                                                        "A3 Sensitive Data Exposure",
                                                                        "A4 XML External Entities (XXE)",
                                                                        "A5 Broken Access Control",
                                                                        "A6 Security Misconfiguration",
                                                                        "A7 Cross-Site Scripting (XSS)",
                                                                        "A8 Insecure Deserialization",
                                                                        "A9 Using Components with Known Vulnerabilities",
                                                                        "A10 Insufficient Logging & Monitoring"
                                                                   };
     

        public string References { get; set; }

        public string OWASPCategory
        {
            get
            {
                if (string.IsNullOrEmpty(References))
                {
                    return null;
                }
                else
                {
                    foreach (var owaspCategory in _owaspCategories)
                    {
                        if (References.Contains($"{OWASP_VERSION}, {owaspCategory}", System.StringComparison.InvariantCultureIgnoreCase))
                        {
                            return $"{OWASP_VERSION}, {owaspCategory}";
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Based on OWASP TOP 10 (2017)
        /// https://cwe.mitre.org/data/definitions/1026.html
        /// </summary>
        public bool IsOWASPTopTen 
        { 
            get 
            {
                return !string.IsNullOrEmpty(OWASPCategory);
            } 
        }
    }
}