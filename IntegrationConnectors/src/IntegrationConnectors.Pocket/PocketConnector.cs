using IntegrationConnectors.Common;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace IntegrationConnectors.Pocket
{
    public class PocketConnector: HttpConnector
    {
        //TODO: connect to 

        public PocketConnector(string baseUrl, string apiKey, AuthenticationType authType) : base(baseUrl, apiKey, authType)
        {
        }

        //public async Task<List<PocketArticle>> GetArticles(string sourceFeed, string targetFeed)
        public async Task GetArticles(string feedUrl, string username=null, string password=null)
        {
            //https://devblogs.microsoft.com/dotnet/feed/
            XmlReader reader;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                XmlUrlResolver resolver = new XmlUrlResolver
                {
                    Credentials = new NetworkCredential(username, password)
                };
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    XmlResolver = resolver
                };
                reader = XmlReader.Create(feedUrl, settings);
            }
            else
            {
                reader = XmlReader.Create(feedUrl);
            }
                
            var feed = SyndicationFeed.Load(reader);
        }
    }
}
