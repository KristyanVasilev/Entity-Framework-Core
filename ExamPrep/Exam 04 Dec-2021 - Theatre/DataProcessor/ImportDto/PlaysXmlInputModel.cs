namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Play")]
    public class PlaysXmlInputModel
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Title { get; set; }

        
        public string Duration { get; set; }

        [Range(0, 10)]
        public float Rating { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [StringLength(700)]
        public string Description { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Screenwriter { get; set; }
    }
}
