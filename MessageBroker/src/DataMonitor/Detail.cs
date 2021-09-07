
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataMonitor
{
    //[XmlArrayItem(ElementName ="detail")]
    public class Detail
    {
        [XmlElement(ElementName = "entitytype")]
        public string Entitytype { get; set; }

        [XmlElement(ElementName = "entityid")]
        public long Entityid { get; set; }

        [XmlElement(ElementName = "providerid", IsNullable = true)]
        public string Providerid { get; set; }

        [XmlElement(ElementName = "providername", IsNullable = true)]
        public string Providername { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "numdaysdelay")]
        public int Numdaysdelay { get; set; }

        [XmlArray("attributes")]
        [XmlArrayItem("attribute", Type = typeof(IncidentAttribute))]
        public List<IncidentAttribute> Attributes { get; set; }

    }
}