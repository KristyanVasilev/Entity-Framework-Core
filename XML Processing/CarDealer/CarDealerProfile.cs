using AutoMapper;
using CarDealer.DTO.Input;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierInputModel, Supplier>();

            this.CreateMap<PartInputModel, Part>();

            this.CreateMap<CustomerInputModel, Customer>();

            this.CreateMap<SalesInputModel, Sale>();
        }
    }
}
