using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTO.Output
{
    [XmlType("Product")]
    public class ProductsOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("buyer")]
        public string BuyerName { get; set; }
    }
}
/*
<Product>
  <name>TRAMADOL HYDROCHLORIDE</name>
  <price>516.48</price>
  <buyer>Brendin Predohl</buyer>
</Product>

 */