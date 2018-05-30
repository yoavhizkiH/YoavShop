using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using YoavShop.Models;

namespace YoavShop.DAL
{
    public class StoreInitializer : DropCreateDatabaseAlways<YoavShopContext>
    {
        protected override void Seed(YoavShopContext context)
        {
            var creditCards = new List<CreditCard>()
            {
                new CreditCard()
                {
                    Id = 1,
                    CardNumber = 123123123,
                    ExiprationMounth = 12,
                    ExiprationYear = 2000
                },
                new CreditCard()
                {
                    Id = 2,
                    CardNumber = 1111111,
                    ExiprationMounth = 12,
                    ExiprationYear = 2000
                }
            };
            context.CreditCards.AddRange(creditCards);

            var UserInfos = new List<UserInfo>()
            {
                new UserInfo()
                {
                    Id = 1,
                    CreditCardId = 1,
                    UserName = "YoavSup"
                },
                new UserInfo()
                {
                    Id = 2,
                    CreditCardId = 2,
                    UserName = "AsafSup"
                },
                new UserInfo()
                {
                    Id = 3,
                    CreditCardId = 3,
                    UserName = "Cust"
                }
            };
            context.UserInfos.AddRange(UserInfos);

            var Suppliers = new List<Supplier>()
            {
                new Supplier()
                {
                    Id = 1,
                    UserInfoId = 1
                },
                new Supplier()
                {
                    Id = 1,
                    UserInfoId = 2
                }
            };
            context.Suppliers.AddRange(Suppliers);

            var customers = new List<Customer>()
            {
                new Customer()
                {
                    UserInfoId = 3
                }
            };
            context.Customers.AddRange(customers);

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

            var Products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "Watch1",
                    CategorieId = 2,
                    Description = "Fake rolex",
                    Price = 5,
                    SupplierId = 1
                }
            };
            context.Products.AddRange(Products);

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