using System.Xml.Serialization;

namespace ProductShop.DTO.Output
{
    [XmlType("Product")]
    public class SoldProductsOutputModel
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
/*    <Product>
        <name>olio activ mouthwash</name>
        <price>206.06</price>
      </Product>
 */