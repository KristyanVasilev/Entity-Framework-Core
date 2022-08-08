namespace Footballers.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class CoachXmlInputModel
    {
        [XmlElement]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [XmlElement]
        [Required]
        public string Nationality { get; set; }

        [XmlArray("Footballers")]
        public FootballerXmlInputModel[] Footballers { get; set; }
    }
}
