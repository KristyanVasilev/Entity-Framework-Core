namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Footballer")]
    public class FootballerXmlInputModel
    {

        [XmlElement]
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        [XmlElement]
        [Required]
        public string ContractStartDate { get; set; }

        [XmlElement]
        [Required]
        public string ContractEndDate { get; set; }

        [XmlElement]
        [Range(0, 4)]
        public int BestSkillType { get; set; }

        [XmlElement]
        [Range(0, 3)]
        public int PositionType { get; set; }
    }
}