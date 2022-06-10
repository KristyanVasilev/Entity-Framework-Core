using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTO.Output
{
    [XmlType("Category")]
    public class CategoryOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("averagePrice")]
        public decimal AveragePrice { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }
}
/*<Category>
    <name>Adult</name>
    <count>22</count>
    <averagePrice>704.41</averagePrice>
    <totalRevenue>15497.02</totalRevenue>
  </Category>

 */