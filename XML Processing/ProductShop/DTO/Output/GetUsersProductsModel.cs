using ProductShop.DTO.Output;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.DTO.Output
{
    [XmlType("User")]
    public class GetUsersProductsModel
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public UserSoldProduct SoldProducts { get; set; }

    }

    [XmlType("SoldProducts")]
    public class UserSoldProduct
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public List<SoldProductsOutputModel> Products { get; set; }
    }

    [XmlType("Users")]
    public class FullModel
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public List<GetUsersProductsModel> Users { get; set; }
    }
}