using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace RpiWebServer.Model
{
    [XmlRoot(ElementName = "PiAction")]
    public class PiAction
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "Direction")]
        public string Direction { get; set; }
        [XmlAttribute(AttributeName = "Length")]
        public string Length { get; set; }
    }

    [XmlRoot(ElementName = "Pin")]
    public class Pin
    {
        [XmlAttribute(AttributeName = "PinNumber")]
        public string PinNumber { get; set; }
        [XmlAttribute(AttributeName = "Direction")]
        public string Direction { get; set; }
        [XmlAttribute(AttributeName = "State")]
        public string State { get; set; }
    }

    [XmlRoot(ElementName = "PinConfig")]
    public class PinConfig
    {
        [XmlElement(ElementName = "Pin")]
        public List<Pin> Pin { get; set; }
    }

    [XmlRoot(ElementName = "Data")]
    public class Data
    {
        [XmlElement(ElementName = "PiActions")]
        public List<PiAction> PiActions { get; set; }
        [XmlAttribute(AttributeName = "Enable")]
        public bool Enable { get; set; }
    }

    [XmlRoot(ElementName = "DataModel")]
    public class DataModel
    {
        [XmlElement(ElementName = "Data")]
        public Data Data { get; set; }

        [XmlElement(ElementName = "PinConfig")]
        public PinConfig PinConfig { get; set; }
    }
}

