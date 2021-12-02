using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Dtos.Input;
using ProductShop.Dtos.Output;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string usersJsonAsString = File.ReadAllText("../../../Datasets/users.json");
            //string productsJsonAsString = File.ReadAllText("../../../Datasets/products.json");
            //string categoriesJsonAsString = File.ReadAllText("../../../Datasets/categories.json");
            //string categoryProuctsJsonAsString = File.ReadAllText("../../../Datasets/categories-products.json");
            ////Console.WriteLine(ImportUsers(context, usersJsonAsString));
            //Console.WriteLine(ImportCategoryProducts(context, categoryProuctsJsonAsString));

            Console.WriteLine(GetUsersWithProducts(context));
        }

        //Problem 1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<UserInputDto> users = JsonConvert.DeserializeObject<IEnumerable<UserInputDto>>(inputJson);
            InitializeMapper();

            IEnumerable<User> mappedUsers = mapper.Map<IEnumerable<User>>(users);

            context.Users.AddRange(mappedUsers);
            context.SaveChanges();

            return $"Successfully imported {mappedUsers.Count()}";
        }

        //Problem 2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<ProductInputDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductInputDto>>(inputJson);
            InitializeMapper();

            var mappedProducts = mapper.Map<IEnumerable<Product>>(products);
            context.Products.AddRange(mappedProducts);
            context.SaveChanges();

            return $"Successfully imported {mappedProducts.Count()}";
        }

        //Problem 3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryInputDto> categories = 
                JsonConvert.DeserializeObject<IEnumerable<CategoryInputDto>>(inputJson)
                .Where(x => !string.IsNullOrEmpty(x.Name));
            InitializeMapper();

            var mappedCategories = mapper.Map<IEnumerable<Category>>(categories);
            context.Categories.AddRange(mappedCategories);
            context.SaveChanges();

            return $"Successfully imported {mappedCategories.Count()}";
        }

        //Problem 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryProductsInputDto> categoryProducts =
                JsonConvert.DeserializeObject<IEnumerable<CategoryProductsInputDto>>(inputJson);
            InitializeMapper();

            var mappedCategoryProduct = mapper.Map<IEnumerable<CategoryProduct>>(categoryProducts);
            context.CategoryProducts.AddRange(mappedCategoryProduct);
            context.SaveChanges();

            return $"Successfully imported {mappedCategoryProduct.Count()}";
        }

        //Problem 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    Name = x.Name,
                    Price = x.Price,
                    Seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()

            };

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string prouctsAsJson = JsonConvert.SerializeObject(products, jsonSettings);

            return prouctsAsJson;
        }

        //Problem 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            InitializeMapper();

            var users = context.Users
                .Include(ps => ps.ProductsSold)
                .Where(u => u.ProductsSold.Any(b => b.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<UserWithSoldProductDto>(mapper.ConfigurationProvider)
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string productsAsJson = JsonConvert.SerializeObject(users, jsonSettings);

            return productsAsJson;
        }

        //Problem 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            InitializeMapper();

            var result = context.Categories
              .OrderByDescending(x => x.CategoryProducts.Count)
              .ProjectTo<CategoryProductsDto>(mapper.ConfigurationProvider)
              .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string resultsAsJson = JsonConvert.SerializeObject(result, jsonSettings);

            return resultsAsJson;

        }

        //Problem 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            InitializeMapper();

            var users = context.Users
                 .Include(x => x.ProductsSold)
                 .ToList()
                 .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
                 .Select(x => new UserProductstDto
                 {
                     FirstName = x.FirstName,
                     LastName = x.LastName,
                     Age = x.Age,
                     SoldProducts = new ProductsDto
                     {
                         Count = x.ProductsSold
                         .Where(p => p.Buyer != null)
                         .Count(),

                         Products = x.ProductsSold
                         .Where(p => p.Buyer != null)
                         .Select(p => new ProductDto
                         {
                             Name = p.Name,
                             Price = p.Price,
                         })
                         .ToList()
                     }
                 })
                 .OrderByDescending(x => x.SoldProducts.Count)
                 .ToList();

            var result = new UsersWithSoldProductsOutputDto
            {
                UsersCount = users.Count(),
                Users = users
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            };
            string resultAsJson = JsonConvert
               .SerializeObject(result,
               jsonSettings);

            return resultAsJson;
        }

        private static void InitializeMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            mapper = new Mapper(mapperConfiguration);
        }
    }
}