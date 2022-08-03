namespace Theatre.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Play")]
    public class PlayXmlExportModel
    {

        [XmlArray("Actors")]
        public ActorXmlExportModel[] Actors { get; set; }


        [XmlAttribute()]
        public string Title { get; set; }


        [XmlAttribute()]
        public string Duration { get; set; }


        [XmlAttribute()]
        public string Rating { get; set; }


        [XmlAttribute()]
        public string Genre { get; set; }
    }
}
