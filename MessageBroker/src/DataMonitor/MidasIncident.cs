using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DataMonitor
{
    [Serializable, XmlRoot("incident")]
    public class MidasIncident
    {
        [XmlElement(ElementName = "id")]
        public long Id { get; set; }

        [XmlElement(ElementName = "incidentDate")]
        public  DateTime Date { get; set; }

        [XmlElement(ElementName = "clientid")]
        public int ClientId { get; set; }

        [XmlElement(ElementName = "clientcode")]
        public string ClientCode { get; set; }

        [XmlElement(ElementName = "vin")]
        public string VIN { get; set; }

        [XmlElement(ElementName = "rego")]
        public string Rego { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "longitude")]
        public double Longitude { get; set; }

        [XmlElement(ElementName = "latitude")]
        public double Latitude { get; set; }

        [XmlElement(ElementName = "incidentcountry")]
        public string Country { get; set; }

        [XmlElement(ElementName = "numberTaxiTrips")]
        public int NumberTaxiTrips { get; set; }

        [XmlElement(ElementName = "numberTrainBusTrips")]
        public int NumberTrainBusTrips { get; set; }

        [XmlElement(ElementName = "numberFlights")]
        public int NumberFlights { get; set; }

        [XmlElement(ElementName = "numberNightsHotel")]
        public int NumberNightsHotel { get; set; }

        [XmlElement(ElementName = "repatriationReuniting")]
        public bool RepatriationReuniting { get; set; }

        [XmlElement(ElementName = "customerEligibility")]
        public string CustomerEligibility { get; set; }

        [XmlElement(ElementName = "customerPayCase")]
        public bool CustomerPayCase { get; set; }

        [XmlElement(ElementName = "goodwillGranted")]
        public bool GoodwillGranted { get; set; }

        [XmlArray("details")]
        [XmlArrayItem("detail", Type = typeof(Detail))]
        public Detail[] details { get; set; }


    }
}