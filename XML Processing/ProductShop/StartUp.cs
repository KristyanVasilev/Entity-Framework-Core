using AutoMapper;
using ProductShop.Data;
using ProductShop.DTO.Input;
using ProductShop.DTO.Output;
using ProductShop.Models;
using ProductShop.XmlFacade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

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

            //var usersXMl = File.ReadAllText("./Datasets/users.xml"); //Task 1
            //var result = ImportUsers(dbContext, usersXMl);

            //var productsXMl = File.ReadAllText("./Datasets/products.xml"); //Task 2
            //var result = ImportProducts(dbContext, productsXMl);

            //var categoriesXMl = File.ReadAllText("./Datasets/categories.xml"); //Task 3
            //var result = ImportCategories(dbContext, categoriesXMl);

            //var categoryProductXMl = File.ReadAllText("./Datasets/categories-products.xml"); //Task 4
            //var result = ImportCategoryProducts(dbContext, categoryProductXMl);

            //var result = GetProductsInRange(dbContext); //Task 5

            //var result = GetSoldProducts(dbContext); //Task 6
            
            //var result = GetCategoriesByProductsCount(dbContext); //Task 7
            
            var result = GetUsersWithProducts(dbContext); //Task 8

            Console.WriteLine(result);
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            const string root = "Users";

            var users = context.Users
               .ToList()
               .Where(x => x.ProductsSold.Any())
               .OrderByDescending(x => x.ProductsSold.Count)
               .Select(x => new GetUsersProductsModel
               {
                   FirstName = x.FirstName,
                   LastName = x.LastName,
                   Age = x.Age,
                   SoldProducts = new UserSoldProduct
                   {
                       Count = x.ProductsSold.Count,
                       Products = x.ProductsSold.Select(s => new SoldProductsOutputModel
                       {
                           Name = s.Name,
                           Price = s.Price
                       })
                       .OrderByDescending(s => s.Price)
                       .ToList()
                   }
               })
               .Take(10)
               .ToList();

            var mainResult = new FullModel
            {
                Count = context.Users.Where(x => x.ProductsSold.Any()).Count(),
                Users = users
            };

            return XmlConverter.Serialize(mainResult, "Users");
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            const string root = "Categories";

            var categories = context.Categories
                .Select(x => new CategoryOutputModel
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(x => x.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(x => x.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToList();

            var xmlResult = XmlConverter.Serialize(categories, root);
            return xmlResult;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            const string root = "Users";

            var users = context.Users
                .Where(x => x.ProductsSold.Count() > 0)
                .Select(x => new UserSoldProductsOutputModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(sp => new SoldProductsOutputModel
                    {
                        Name = sp.Name,
                        Price = sp.Price
                    }).ToArray()
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToList();

            var xmlResult = XmlConverter.Serialize(users, root);
            return xmlResult;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            const string root = "Products";

            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new ProductsOutputModel
                {
                    Name = x.Name,
                    Price = x.Price,
                    BuyerName = x.Buyer.FirstName + ' ' + x.Buyer.LastName
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToList();


            var xmlResult = XmlConverter.Serialize(products, root);
            return xmlResult;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            const string root = "CategoryProducts";
            InitializeAutoMapper();

            var categoriesIds = context.Categories.Select(x => x.Id).ToList();
            var productsIds = context.Products.Select(x => x.Id).ToList();

            var dtoCategoryProducts = XmlConverter.Deserializer<CategoriesProductInputModel>(inputXml, root);
            var filteredCategoriesProducts = dtoCategoryProducts
                .Where(x => categoriesIds.Contains(x.CategoryId) && productsIds.Contains(x.ProductId))
                .ToList();

            var categoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(filteredCategoriesProducts);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            const string root = "Categories";
            InitializeAutoMapper();

            var dtoCategories = XmlConverter.Deserializer<CategoriesInputModel>(inputXml, root);
            var filteredCategories = dtoCategories.Where(x => x.Name != null).ToList();

            var categories = mapper.Map<IEnumerable<Category>>(filteredCategories);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            const string root = "Products";
            InitializeAutoMapper();

            var dtoProducts = XmlConverter.Deserializer<ProductInputModel>(inputXml, root);

            var products = mapper.Map<IEnumerable<Product>>(dtoProducts);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            const string root = "Users";
            InitializeAutoMapper();

            var dtoUsers = XmlConverter.Deserializer<UsersInputModel>(inputXml, root);

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