namespace YoavShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 10, unicode: false),
                        Password = c.String(nullable: false),
                        LastName = c.String(),
                        FirstName = c.String(),
                        CardNumber = c.Long(nullable: false),
                        ExiprationYear = c.Int(nullable: false),
                        ExiprationMounth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        MoneyPaid = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Supplier_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Supplier", t => t.Supplier_UserId)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.CustomerId)
                .Index(t => t.Supplier_UserId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Int(nullable: false),
                        ProductCategorieId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategorie", t => t.ProductCategorieId, cascadeDelete: true)
                .ForeignKey("dbo.Supplier", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.ProductCategorieId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.ProductCategorie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 10, unicode: false),
                        Password = c.String(nullable: false),
                        LastName = c.String(),
                        FirstName = c.String(),
                        CardNumber = c.Long(nullable: false),
                        ExiprationYear = c.Int(nullable: false),
                        ExiprationMounth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transaction", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "SupplierId", "dbo.Supplier");
            DropForeignKey("dbo.Transaction", "Supplier_UserId", "dbo.Supplier");
            DropForeignKey("dbo.Product", "ProductCategorieId", "dbo.ProductCategorie");
            DropForeignKey("dbo.Transaction", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Supplier", new[] { "UserName" });
            DropIndex("dbo.Product", new[] { "SupplierId" });
            DropIndex("dbo.Product", new[] { "ProductCategorieId" });
            DropIndex("dbo.Transaction", new[] { "Supplier_UserId" });
            DropIndex("dbo.Transaction", new[] { "CustomerId" });
            DropIndex("dbo.Transaction", new[] { "ProductId" });
            DropIndex("dbo.Customer", new[] { "UserName" });
            DropTable("dbo.Supplier");
            DropTable("dbo.ProductCategorie");
            DropTable("dbo.Product");
            DropTable("dbo.Transaction");
            DropTable("dbo.Customer");
        }
    }
}
