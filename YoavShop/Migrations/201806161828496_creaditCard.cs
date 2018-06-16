namespace YoavShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creaditCard : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customer", "CardNumber", c => c.String());
            AlterColumn("dbo.Supplier", "CardNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Supplier", "CardNumber", c => c.Long(nullable: false));
            AlterColumn("dbo.Customer", "CardNumber", c => c.Long(nullable: false));
        }
    }
}
