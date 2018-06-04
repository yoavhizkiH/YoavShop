namespace YoavShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMapLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapLocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlaceName = c.String(),
                        GeoLong = c.Double(nullable: false),
                        GeoLat = c.Double(nullable: false),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MapLocation");
        }
    }
}
