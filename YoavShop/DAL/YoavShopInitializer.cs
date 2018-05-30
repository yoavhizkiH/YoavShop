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
            var suppliers = new List<Supplier>();
            var customers = new List<Customer>();
            var categories = new List<ProductCategorie>();
            var products = new List<Product>();
            var transactions = new List<Transaction>();

            for (int i = 1; i < 15; i++)
            {
                suppliers.Add(new Supplier()
                {
                    UserId = i,
                    FirstName = $"FirstName{i+15}",
                    LastName = $"LastName{i+15}",
                    CardNumber = i * 3,
                    ExiprationMounth = i % 11 + 1,
                    ExiprationYear = i + 2000,
                    UserName = $"User{i+15}",
                    Password = "Password1!"
                });
                customers.Add(new Customer()
                {
                    UserId = i,
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    CardNumber = i * 123,
                    ExiprationMounth = i % 11 + 1,
                    ExiprationYear = i + 2000,
                    UserName = $"User{i}",
                    Password = "Password1!"
                });
                categories.Add(new ProductCategorie()
                {
                    Id = i,
                    Name = $"Jewlery{i}"
                });
                products.Add(new Product()
                {
                    Id = i,
                    Name = $"Product{i}",
                    ProductCategorieId = i,
                    Description = $"{i}Description{i}",
                    Price = i*15,
                    SupplierId = i
                });
                transactions.Add(new Transaction()
                {
                    CustomerId = i,
                    MoneyPaid = i*12,
                    ProductId = i,
                    TimeStamp = DateTime.Now.AddYears(i)
                });
            }

            context.Suppliers.AddRange(suppliers);
            context.Customers.AddRange(customers);
            context.ProductCategories.AddRange(categories);
            context.Products.AddRange(products);
            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }
    }
}