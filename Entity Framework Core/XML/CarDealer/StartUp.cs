namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTO.Export;
    using CarDealer.DTO.Import;
    using CarDealer.Models;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class StartUp
    {
        private static readonly IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");
            //string result = ImportSales(context, inputXml);
            string result = GetSalesWithAppliedDiscount(context);
            Console.WriteLine(result);
        }

        //Problem 9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportSupplierDto[] dtos = (ImportSupplierDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();
            foreach (ImportSupplierDto supplierDto in dtos)
            {
                Supplier s = new Supplier()
                {
                    Name = supplierDto.Name,
                    IsImporter = bool.Parse(supplierDto.IsImporter)
                };

                suppliers.Add(s);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportPartDto[] dtos = (ImportPartDto[])xmlSerializer.Deserialize(stringReader);
            ICollection<Part> parts = new HashSet<Part>();
            foreach (ImportPartDto partDto in dtos)
            {
                Supplier supplier = context.Suppliers
                    .Find(partDto.SupplierId);

                if (supplier == null)
                {
                    continue;
                }

                Part p = new Part()
                {
                    Name = partDto.Name,
                    Price = decimal.Parse(partDto.Price),
                    Quantity = partDto.Quantity,
                    Supplier = supplier
                };

                parts.Add(p);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = GenerateXmlSerializer("Cars", typeof(ImportCarDto[]));

            using StringReader stringReader = new StringReader(inputXml);
            ImportCarDto[] carDtos = (ImportCarDto[])xmlSerializer.Deserialize(stringReader);
            ICollection<Car> cars = new HashSet<Car>();
            //ICollection<PartCar> partCar = new HashSet<PartCar>();

            foreach (ImportCarDto carDto in carDtos)
            {
                Car c = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance
                };

                ICollection<PartCar> currPartsCar = new HashSet<PartCar>();
                foreach (int partId in carDto.Parts.Select(p => p.Id).Distinct())
                {
                    Part part = context.Parts
                        .Find(partId);

                    if (part == null)
                    {
                        continue;
                    }

                    PartCar carPart = new PartCar()
                    {
                        Car = c,
                        Part = part
                    };

                    currPartsCar.Add(carPart);
                }

                c.PartCars = currPartsCar;
                cars.Add(c);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = GenerateXmlSerializer("Customers", typeof(ImportCustomerDto[]));
            
            using StringReader stringReader = new StringReader(inputXml);
            ImportCustomerDto[] customerDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(stringReader);
            ICollection<Customer> customers = new HashSet<Customer>();

            foreach (ImportCustomerDto dto in customerDtos)
            {
                Customer c = new Customer()
                {
                    Name = dto.Name,
                    BirthDate = DateTime.Parse(dto.BirthDate),
                    IsYoungDriver = bool.Parse(dto.IsYoungDriver)
                };

                customers.Add(c);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = GenerateXmlSerializer("Sales", typeof(ImportSaleDto[]));

            using StringReader stringReader = new StringReader(inputXml);
            ImportSaleDto[] saleDtos = (ImportSaleDto[])xmlSerializer.Deserialize(stringReader);
            ICollection<Sale> sales = new HashSet<Sale>();

            foreach (ImportSaleDto dto in saleDtos)
            {
                Car car = context.Cars
                    .Find(dto.CarId);

                if (car == null)
                {
                    continue;
                }

                Sale s = new Sale()
                {
                    Car = car,
                    CustomerId = dto.CustomerId,
                    Discount = dto.Discount
                };

                sales.Add(s);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //Problem 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer = 
                GenerateXmlSerializer("cars", typeof(ExportCarsWithDistanceDto[]));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportCarsWithDistanceDto[] carsDtos = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarsWithDistanceDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance.ToString()
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, carsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer = 
                GenerateXmlSerializer("cars", typeof(ExportCarsBWMDto[]));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(String.Empty, String.Empty);


            var bmwCarDtos = context
                .Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new ExportCarsBWMDto()
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, bmwCarDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer = 
                GenerateXmlSerializer("suppliers", typeof(ExportLocalSuppliersDto[]));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(String.Empty, String.Empty);


            var suppliersDtos = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new ExportLocalSuppliersDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, suppliersDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer =
                GenerateXmlSerializer("cars", typeof(ExportCarsWithPartsDto[]));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var carsWithParts = context.Cars
                .Select(c => new ExportCarsWithPartsDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = context.PartCars
                        .Where(pc => pc.CarId == c.Id)
                        .Select(pc => new PartsDto
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price
                        })
                        .OrderByDescending(cp => cp.Price)
                        .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            xmlSerializer.Serialize(stringWriter, carsWithParts, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer =
                GenerateXmlSerializer("customers", typeof(ExportTotalSalesDto[]));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var totalSales = context.Customers
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new ExportTotalSalesDto
                {
                    Name = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            xmlSerializer.Serialize(stringWriter, totalSales, namespaces);

            return sb.ToString().TrimEnd();

        }

        //Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer =
                GenerateXmlSerializer("sales", typeof(ExportSalesWithDiscountDto[]));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var salesWithDiscount = context.Sales
                .Select(s => new ExportSalesWithDiscountDto
                {
                    Car = new CarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    WithDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100).ToString()
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, salesWithDiscount, namespaces);

            return sb.ToString().TrimEnd();
        }

        private static XmlSerializer GenerateXmlSerializer(string rootName, Type dtoType)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(dtoType, xmlRoot);

            return xmlSerializer;
        }
    }
}