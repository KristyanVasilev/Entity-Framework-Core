using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class CarsInputModel
    {

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public IEnumerable<int> PartsId { get; set; }
    }
}
