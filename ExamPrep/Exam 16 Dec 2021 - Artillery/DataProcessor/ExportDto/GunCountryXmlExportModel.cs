namespace Artillery.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Country")]
    public class GunCountryXmlExportModel
    {

        [XmlAttribute()]
        public string Country { get; set; }


        [XmlAttribute()]
        public int ArmySize { get; set; }
    }
}
