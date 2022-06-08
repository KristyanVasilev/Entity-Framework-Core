using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DataTrasnferObjects
{
    public class ProductInputModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int SellerId { get; set; }
        public int? BuyerId { get; set; }
    }
}
//"Name": "SNORING HP",
//    "Price": 53.59,
//    "SellerId": 24,
//    "BuyerId": 30
