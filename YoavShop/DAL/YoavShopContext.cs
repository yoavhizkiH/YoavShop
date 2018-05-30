using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using YoavShop.Models;

namespace YoavShop.DAL
{
    public class YoavShopContext : DbContext
    {
        //public DbSet<CreditCard> CreditCards { get; set; }
        //public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<ProductCategorie> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public YoavShopContext() : base(nameof(YoavShopContext))
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}