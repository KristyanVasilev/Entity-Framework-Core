namespace Theatre.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Actors")]
    public partial class ActorXmlExportModel
    {

        [XmlAttribute()]
        public string FullName { get; set; }
  

        [XmlAttribute()]
        public string MainCharacter { get; set; }
    }
}
