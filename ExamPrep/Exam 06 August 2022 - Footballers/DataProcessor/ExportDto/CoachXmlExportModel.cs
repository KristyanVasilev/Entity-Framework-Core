namespace Footballers.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class CoachXmlExportModel
    {
        [XmlElement("CoachName")]
        public string CoachName { get; set; }

        [XmlAttribute("FootballersCount")]
        public string FootballersCount { get; set; }

        [XmlArray("Footballers")]
        public FootballerXmlExportModel[] Footballers { get; set; }
    }
}








