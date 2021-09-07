using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RssFeed.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RssFeedController : ControllerBase
    {
        private readonly ILogger<RssFeedController> _logger;

        public RssFeedController(ILogger<RssFeedController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public SyndicationFeed Get(string feedUrl, string username, string password)
        {
            //Test: https://devblogs.microsoft.com/dotnet/feed/
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

            return feed;
        }
    }
}
