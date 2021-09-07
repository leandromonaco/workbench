using System.Xml.Serialization;

namespace DataMonitor
{
    public class IncidentAttribute
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}