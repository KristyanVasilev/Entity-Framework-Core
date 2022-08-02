namespace VaporStore.DataProcessor.Dto.Export
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UserXmlExportModel
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray("Purchases")]
        public PurchaseExportModelModel[] Purchases { get; set; }

        public decimal TotalSpent { get; set; }
    }
}
