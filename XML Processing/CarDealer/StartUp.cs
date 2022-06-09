using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Input;
using CarDealer.DTO.Output;
using CarDealer.Models;
using CarDealer.XmlHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var dbContext = new CarDealerContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //var suplierXMl = File.ReadAllText("./Datasets/suppliers.xml"); //Task 1
            //var result = ImportSuppliers(dbContext, suplierXMl);

            //var partsXMl = File.ReadAllText("./Datasets/parts.xml"); //Task 2
            //var result = ImportParts(dbContext, partsXMl);

            //var carsXMl = File.ReadAllText("./Datasets/cars.xml"); //Task 3
            //var result = ImportCars(dbContext, carsXMl);

            //var customersXMl = File.ReadAllText("./Datasets/customers.xml"); //Task 4
            //var result = ImportCustomers(dbContext, customersXMl);

            //var salesXMl = File.ReadAllText("./Datasets/sales.xml"); //Task 5
            //var result = ImportSales(dbContext, salesXMl);

            //var result = GetCarsWithDistance(dbContext); //Task 6

            //var result = GetCarsFromMakeBmw(dbContext); //Task 7

            //var result = GetLocalSuppliers(dbContext); //Task 8

            //var result = GetCarsWithTheirListOfParts(dbContext); //Task 9

            //var result = GetTotalSalesByCustomer(dbContext); //Task 10
            
            var result = GetSalesWithAppliedDiscount(dbContext); //Task 11

            Console.WriteLine(result);
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            const string root = "sales";

            var sales = context.Sales
                .Select(x => new DiscountOutputModel
                {
                    Car = new CarInfoOutputModel
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = x.Car.PartCars.Sum(c => c.Part.Price),
                    PriceWithtDiscount = x.Car.PartCars.Sum(c => c.Part.Price) - x.Car.PartCars.Sum(c => c.Part.Price) * x.Discount / 100m,

                })
                .ToList();

            var xmlResult = XmlConverter.Serialize(sales, root);
            return xmlResult;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            const string root = "customers";

            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new SalesOutputModel
                {
                    Name = x.Name,
                    BoughtCars = x.Sales.Count(),
                    MoneySpent = x.Sales.Select(s => s.Car).SelectMany(s => s.PartCars).Sum(p => p.Part.Price)
                })
                .OrderByDescending(x => x.MoneySpent)
                .ToList();

            var xmlResult = XmlConverter.Serialize(customers, root);
            return xmlResult;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            const string root = "cars";

            var cars = context.Cars
                .Select(x => new CarPartsOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    CarPartInfo = x.PartCars.Select(p => new CarPartInfoOutputModel
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToList();

            var xmlResult = XmlConverter.Serialize(cars, root);
            return xmlResult;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            const string root = "suppliers";

            var cars = context.Suppliers
                .Where(x => x.IsImporter != true)
                .Select(x => new SuppliersOutputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Parts = x.Parts.Count()
                })
                .ToList();

            var xmlResult = XmlConverter.Serialize(cars, root);

            return xmlResult;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            const string root = "cars";

            var cars = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new MakeOutputModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            var xmlResult = XmlConverter.Serialize(cars, root);

            return xmlResult;
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            const string root = "cars";

            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .Select(x => new CarOutputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ToList();

            var xmlResult = XmlConverter.Serialize(cars, root);

            return xmlResult;
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            const string root = "Sales";
            InitializeAutoMapper();

            var carsIds = context.Cars
              .Select(x => x.Id)
              .ToArray();

            var dtoSales = XmlConverter.Deserializer<SalesInputModel>(inputXml, root);

            var filteredSales = dtoSales.Where(x => carsIds.Contains(x.CarId));

            var sales = mapper.Map<IEnumerable<Sale>>(filteredSales);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            const string root = "Customers";
            InitializeAutoMapper();

            var dtoCustomers = XmlConverter.Deserializer<CustomerInputModel>(inputXml, root);

            var customers = mapper.Map<IEnumerable<Customer>>(dtoCustomers);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml) //problem with judge??
        {
            const string root = "Cars";

            var dtoCars = XmlConverter.Deserializer<CarInputModel>(inputXml, root);
            var allParts = context.Parts.Select(x => x.Id).ToList();

            var cars = new List<Car>();

            foreach (var currcar in dtoCars)
            {
                var distinctedParts = currcar.CarPartsInputModel.Select(x => x.Id).Distinct();
                var parts = distinctedParts
                    .Intersect(allParts);

                var car = new Car
                {
                    Make = currcar.Make,
                    Model = currcar.Model,
                    TravelledDistance = currcar.TraveledDistance,
                };

                foreach (var part in parts)
                {
                    var partCar = new PartCar
                    {
                        PartId = part
                    };

                    car.PartCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            const string root = "Parts";
            InitializeAutoMapper();

            var suppliesIds = context.Suppliers
               .Select(x => x.Id)
               .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(PartInputModel[]), new XmlRootAttribute(root));
            var textReader = new StringReader(inputXml);

            var dtoParts = xmlSerializer.Deserialize(textReader) as PartInputModel[];
            var filteredParts = dtoParts.Where(p => suppliesIds.Contains(p.SupplierId));

            var parts = mapper.Map<IEnumerable<Part>>(filteredParts);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            const string root = "Suppliers";
            InitializeAutoMapper();

            var xmlSerializer = new XmlSerializer(typeof(SupplierInputModel[]), new XmlRootAttribute(root));
            var textReader = new StringReader(inputXml);

            var dtoSuppliers = xmlSerializer.Deserialize(textReader) as SupplierInputModel[];

            var suppliers = mapper.Map<IEnumerable<Supplier>>(dtoSuppliers);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";
        }

        private static void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            mapper = config.CreateMapper();
        }
    }
}