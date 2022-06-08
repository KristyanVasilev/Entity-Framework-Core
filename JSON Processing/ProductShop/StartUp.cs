using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DataTrasnferObjects;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var dbContext = new ProductShopContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //string jsonInput = File.ReadAllText("../../../Datasets/users.json"); //Task 1
            //var result = ImportUsers(dbContext, jsonInput);

            //string jsonInput = File.ReadAllText("../../../Datasets/products.json"); //Task 2
            //var result = ImportProducts(dbContext, jsonInput);

            //string jsonInput = File.ReadAllText("../../../Datasets/categories.json"); //Task 3
            //var result = ImportCategories(dbContext, jsonInput);

            //string jsonInput = File.ReadAllText("../../../Datasets/categories-products.json"); //Task 4
            //var result = ImportCategoryProducts(dbContext, jsonInput);

            //var result = GetProductsInRange(dbContext); //Task 5

            //var result = GetSoldProducts(dbContext); //Task 6

            //var result = GetCategoriesByProductsCount(dbContext); //Task 7

            var result = GetUsersWithProducts(dbContext); //Task 8

            Console.WriteLine(result);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(x => x.ProductsSold)
                .ToList()
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new
                    {
                        count = x.ProductsSold
                        .Where(p => p.BuyerId != null).Count(),
                        products = x.ProductsSold
                        .Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    }
                })
                .OrderByDescending(x => x.soldProducts.count)
                .ToList();

            var resultObject = new
            {
                usersCount = users.Count,
                users = users
            };

            var jsonSerialierSetting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            var jsonResult = JsonConvert.SerializeObject(resultObject, Formatting.Indented, jsonSerialierSetting);
            return jsonResult;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Average(p => p.Product.Price).ToString("F2"),
                    totalRevenue = c.CategoryProducts.Sum(p => p.Product.Price).ToString("F2")
                })
                .OrderByDescending(c => c.productsCount)
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(categories, Formatting.Indented);
            return jsonResult;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(user => new
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    soldProducts = user.ProductsSold
                    .Where(p => p.BuyerId != null)
                    .Select(b => new
                    {
                        name = b.Name,
                        price = b.Price,
                        buyerFirstName = b.Buyer.FirstName,
                        buyerLastName = b.Buyer.LastName,
                    })
                    .ToList()
                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstName)
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(users, Formatting.Indented);
            return jsonResult;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + ' ' + x.Seller.LastName
                })
                .OrderBy(x => x.price)
                .ToList();

            var jsonResult = JsonConvert.SerializeObject(products, Formatting.Indented);
            return jsonResult;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();
            var dtoCategoriesProducts = JsonConvert.DeserializeObject<IEnumerable<CategoriesProductInputModel>>(inputJson);

            var categoriesProducts = mapper.Map<IEnumerable<CategoryProduct>>(dtoCategoriesProducts);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();
            var dtoCategories = JsonConvert.DeserializeObject<IEnumerable<CategoriesInputModel>>(inputJson)
                .Where(x => x.Name != null);

            var categories = mapper.Map<IEnumerable<Category>>(dtoCategories);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();
            var dtoProducts = JsonConvert.DeserializeObject<IEnumerable<ProductInputModel>>(inputJson);

            var products = mapper.Map<IEnumerable<Product>>(dtoProducts);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeAutoMapper();

            var dtoUsers = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);

            var users = mapper.Map<IEnumerable<User>>(dtoUsers);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        private static void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            mapper = config.CreateMapper();
        }
    }
}