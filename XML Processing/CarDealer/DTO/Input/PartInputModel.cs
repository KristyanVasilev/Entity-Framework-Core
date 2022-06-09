using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO.Input
{
    [XmlType("Part")]
    public class PartInputModel
    {

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("Price")]
        public decimal price { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("supplierId")]
        public int SupplierId { get; set; }
    }
}

//< Part >
//    < name > Bonnet / hood </ name >
//    < price > 1001.34 </ price >
//    < quantity > 10 </ quantity >
//    < supplierId > 17 </ supplierId >
//</ Part >
