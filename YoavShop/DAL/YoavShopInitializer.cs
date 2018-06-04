using System;
using System.Collections.Generic;
using System.Data.Entity;
using YoavShop.Models;

namespace YoavShop.DAL
{
    public class YoavShopInitializer : DropCreateDatabaseIfModelChanges<YoavShopContext>
    {
        protected override void Seed(YoavShopContext context)
        {
            var suppliers = new List<Supplier>();
            var customers = new List<Customer>();
            var categories = new List<ProductCategorie>();
            var products = new List<Product>();
            var transactions = new List<Transaction>();
            var branches = new List<MapLocation>();

            for (var i = 1; i < 15; i++)
            {
                suppliers.Add(new Supplier
                {
                    Id = i,
                    FirstName = $"FirstName{i + 15}",
                    LastName = $"LastName{i + 15}",
                    CardNumber = i * 3,
                    ExiprationMounth = i % 11 + 1,
                    ExiprationYear = i + 2000,
                    UserName = $"User{i + 15}",
                    Password = "Password1!"
                });
                customers.Add(new Customer
                {
                    Id = i,
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    CardNumber = i * 123,
                    ExiprationMounth = i % 11 + 1,
                    ExiprationYear = i + 2000,
                    UserName = $"User{i}",
                    Password = "Password1!"
                });

                products.Add(new Product
                {
                    Id = i,
                    Name = $"Product{i}",
                    ProductCategorieId = i % 3 + 1,
                    Color = (ProductColor) Enum.GetValues(typeof(ProductColor)).GetValue(i % 7),
                    Amount = i * 20,
                    Description = $"{i}Description{i}",
                    Price = i * 15,
                    SupplierId = i
                });
                transactions.Add(new Transaction
                {
                    CustomerId = i,
                    MoneyPaid = i * 12,
                    ProductId = i,
                    TimeStamp = DateTime.Now
                });
            }

            categories.AddRange(new List<ProductCategorie>
                {
                    new ProductCategorie
                    {
                        Id = 1,
                        Name = "Earings"
                    },
                    new ProductCategorie
                    {
                        Id = 2,
                        Name = "Watches"
                    },
                    new ProductCategorie
                    {
                        Id = 3,
                        Name = "Necklesses"
                    }
                }
            );

            branches.AddRange(new List<MapLocation>
                {
                    new MapLocation
                    {
                        Id = 1,
                        PlaceName = "סניף ראשון לציון",
                        GeoLong = 31.969365,
                        GeoLat = 34.772289,
                        Info = "שעות פתיחה: 10:00 - 17:00"
                    },
                    new MapLocation
                    {
                        Id = 2,
                        PlaceName = "סניף תל אביב",
                        GeoLong = 32.086167,
                        GeoLat = 34.790039,
                        Info = "שעות פתיחה: 11:00 - 11:01"
                    },
                    new MapLocation
                    {
                        Id = 3,
                        PlaceName = "סניף ירושלים",
                        GeoLong = 31.768809,
                        GeoLat = 35.212450,
                        Info = "שעות פתיחה: 8:00 - 13:00"
                    },
                    new MapLocation
                    {
                        Id = 4,
                        PlaceName = "סניף המכללה למנהל",
                        GeoLong = 31.969929,
                        GeoLat = 34.772283,
                        Info = "שעות פתיחה: 15:00 - 22:00"
                    }
                }
            );

            context.Suppliers.AddRange(suppliers);
            context.Customers.AddRange(customers);
            context.ProductCategories.AddRange(categories);
            context.SaveChanges();
            context.Products.AddRange(products);
            context.Transactions.AddRange(transactions);
            context.MapLocations.AddRange(branches);
            context.SaveChanges();
        }
    }
}