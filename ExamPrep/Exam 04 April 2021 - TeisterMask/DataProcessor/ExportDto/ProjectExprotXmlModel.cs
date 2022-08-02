namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Project")]
    public partial class ProjectExprotXmlModel
    {
        [XmlAttributeAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public ProjectTaskExprotXmlModel[] Tasks { get; set; }
    }
}
