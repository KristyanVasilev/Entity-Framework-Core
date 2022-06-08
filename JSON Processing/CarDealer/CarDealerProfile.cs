using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SuppliersInputModel, Supplier>();

            this.CreateMap<PartsInputModel, Part>();

            this.CreateMap<CustomersInputModel, Customer>();

            //this.CreateMap<CarsInputModel, Car>();

            this.CreateMap<SalesInputModel, Sale>();
        }
    }
}
