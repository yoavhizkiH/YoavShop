using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using YoavShop.Models;

namespace YoavShop.DAL
{
    public class YoavShopInitializer : DropCreateDatabaseIfModelChanges<YoavShopContext>
    {
        protected override void Seed(YoavShopContext context)
        {
            var Suppliers = new List<Supplier>()
            {
                new Supplier()
                {
                    UserId = 1,
                        CardNumber = 123123123,
                        ExiprationMounth = 12,
                        ExiprationYear = 2000,
                                        UserName = "YoavSup",
                        Password = "123"
                },
                new Supplier()
                {
                    UserId = 2,

                        CardNumber = 1111111,
                        ExiprationMounth = 12,
                        ExiprationYear = 2000,
                        UserName = "AsafSup",
                        Password = "123"
                }
            };
            context.Suppliers.AddRange(Suppliers);
            context.SaveChanges();

            var customers = new List<Customer>()
            {
                new Customer()
                {
                    UserId = 1,

                        CardNumber = 1111111,
                        ExiprationMounth = 12,
                        ExiprationYear = 2000,
                        UserName = "Cust",
                        Password = "123"
                }
            };
            context.Customers.AddRange(customers);
            context.SaveChanges();

            var categories = new List<ProductCategorie>()
            {
                new ProductCategorie()
                {
                    Id = 1,
                    Name = "MenJewlery"
                },
                new ProductCategorie()
                {
                    Id = 2,
                    Name = "Watches"
                }
            };
            context.ProductCategories.AddRange(categories);
            context.SaveChanges();

            var Products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "Watch1",
                    ProductCategorieId = 2,
                    Description = "Fake rolex",
                    Price = 5,
                    SupplierId = 1
                }
            };
            context.Products.AddRange(Products);
            context.SaveChanges();

            var transactions = new List<Transaction>()
            {
                new Transaction()
                {
                    CustomerId = 1,
                    MoneyPaid = 3,
                    ProductId = 1,
                    TimeStamp = DateTime.Now
                }
            };
            context.Transactions.AddRange(transactions);

            context.SaveChanges();
        }
    }
}