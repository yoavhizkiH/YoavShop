namespace YoavShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductAmountAndColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Amount", c => c.Int(nullable: false));
            AddColumn("dbo.Product", "Color", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Color");
            DropColumn("dbo.Product", "Amount");
        }
    }
}
