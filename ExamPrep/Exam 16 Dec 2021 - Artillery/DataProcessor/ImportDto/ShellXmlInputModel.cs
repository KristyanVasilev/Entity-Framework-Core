namespace Artillery.DataProcessor.ImportDto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Shell")]
    public class ShellXmlInputModel
    {
        [XmlElement]
        [Range(2, 1680)]
        public double ShellWeight { get; set; }

        [XmlElement]
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string Caliber { get; set; }
    }
}
