namespace YoavShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedOverrideUserInfo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Customer", new[] { "UserName" });
            DropIndex("dbo.Supplier", new[] { "UserName" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Supplier", "UserName", unique: true);
            CreateIndex("dbo.Customer", "UserName", unique: true);
        }
    }
}
