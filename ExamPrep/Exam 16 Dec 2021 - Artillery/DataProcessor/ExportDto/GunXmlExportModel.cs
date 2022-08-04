namespace Artillery.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Gun")]
    public class GunXmlExportModel
    {
        [XmlAttribute()]
        public string Manufacturer { get; set; }

        [XmlAttribute()]
        public string GunType { get; set; }


        [XmlAttribute()]
        public int GunWeight { get; set; }

        [XmlAttribute()]
        public double BarrelLength { get; set; }


        [XmlAttribute()]
        public int Range { get; set; }

        [XmlArray("Countries")]
        public GunCountryXmlExportModel[] Countries { get; set; }
    }
}