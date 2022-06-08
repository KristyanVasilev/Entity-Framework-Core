using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

            //string jsonInput = File.ReadAllText("../../../Datasets/suppliers.json"); //Task 1
            //var result = ImportSuppliers(dbContext, jsonInput);

            //string jsonInput = File.ReadAllText("../../../Datasets/parts.json"); //Task 2
            //var result = ImportParts(dbContext, jsonInput);

            //string jsonInput = File.ReadAllText("../../../Datasets/cars.json"); //Task 3
            //var result = ImportCars(dbContext, jsonInput);

            //string jsonInput = File.ReadAllText("../../../Datasets/customers.json"); //Task 4
            //var result = ImportCustomers(dbContext, jsonInput);

            //string jsonInput = File.ReadAllText("../../../Datasets/sales.json"); //Task 5
            //var result = ImportSales(dbContext, jsonInput);

            //var result = GetOrderedCustomers(dbContext); //Task 6

            //var result = GetCarsFromMakeToyota(dbContext); //Task 7

            //var result = GetLocalSuppliers(dbContext); //Task 8

            //var result = GetCarsWithTheirListOfParts(dbContext); //Task 9

            //var result = GetTotalSalesByCustomer(dbContext); //Task 10

            var result = GetSalesWithAppliedDiscount(dbContext); //Task 11

            Console.WriteLine(result);
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //var sales = context.Sales
            //    .Take(10)
            //    .Select(x => new
            //    {
            //        Car = new
            //        {
            //            Make = x.Car.Make,
            //            Model = x.Car.Model,
            //            TravelledDistance = x.Car.TravelledDistance
            //        },
            //        customerName = x.Customer.Name,
            //        Discount = x.Discount,
            //        price = x.Car.PartCars
            //            .Sum(y => y.Part.Price)
            //            .ToString("f2"),
            //        priceWithoutDiscount = (x.Car.PartCars.Sum(y => y.Part.Price) - x.Car.PartCars.Sum(y => y.Part.Price) * x.Discount / 100).ToString("f2")
            //    })
            //    .ToList();

            //var jsonResult = JsonConvert.SerializeObject(sales, Formatting.Indented);
            //return jsonResult;

            var sales = context.Sales
                .Take(10)
                .Select(x => new
                {
                    x.Car.Make,
                    x.Car.Model,
                    x.Car.TravelledDistance,
                    x.Customer.Name,
                    x.Discount,
                    price = x.Car.PartCars.Sum(y => y.Part.Price).ToString("f2"),
                    priceWithoutDiscount = (x.Car.PartCars.Sum(y => y.Part.Price) - x.Car.PartCars.Sum(y => y.Part.Price) * x.Discount / 100).ToString("f2")
                })
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            File.WriteAllText("../../../sales-discounts.json", json);

            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Count() > 0)
                .Select(x => new
                {
                    fullName = x.Name,
                    boughtCars = x.Sales.Count(),
                    spentMoney = x.Sales.Sum(y => y.Car.PartCars.Sum(s => s.Part.Price))
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return jsonResult;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new
                {
                    car = new
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance,
                    },
                    Parts =
                        x.PartCars.Select(p => new
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price.ToString("f2")
                        })
                })
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return jsonResult;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count()
                })
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            return jsonResult;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return jsonResult;
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    IsYoungDriver = x.IsYoungDriver
                })
                .OrderBy(x => x.BirthDate)
                .ThenByDescending(x => x.IsYoungDriver)
                .ToList();

            var settings = new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy"
            };

            var jsonResult = JsonConvert.SerializeObject(customers, Formatting.Indented, settings);
            return jsonResult;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoSales = JsonConvert.DeserializeObject<IEnumerable<SalesInputModel>>(inputJson);

            var sales = mapper.Map<IEnumerable<Sale>>(dtoSales);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoCustomers = JsonConvert.DeserializeObject<IEnumerable<CustomersInputModel>>(inputJson);

            var customers = mapper.Map<IEnumerable<Customer>>(dtoCustomers);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoCars = JsonConvert.DeserializeObject<IEnumerable<CarsInputModel>>(inputJson);

            var cars = new List<Car>();

            foreach (var car in dtoCars)
            {
                var currCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,
                };

                foreach (var partId in car.PartsId.Distinct())
                {
                    currCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                cars.Add(currCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();

            var suppliesIds = context.Suppliers
               .Select(x => x.Id)
               .ToArray();

            var dtoParts = JsonConvert.DeserializeObject<IEnumerable<PartsInputModel>>(inputJson)
                .Where(s => suppliesIds.Contains(s.SupplierId));

            var parts = mapper.Map<IEnumerable<Part>>(dtoParts);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoSuppliers = JsonConvert.DeserializeObject<IEnumerable<SuppliersInputModel>>(inputJson);

            var suppliers = mapper.Map<IEnumerable<Supplier>>(dtoSuppliers);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();


            return $"Successfully imported {suppliers.Count()}.";
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