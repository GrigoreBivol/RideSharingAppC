namespace RideSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelUpdate1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Drivers", "OnLine");
            DropColumn("dbo.Drivers", "OnRide");
            DropColumn("dbo.Trips", "Total");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trips", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Drivers", "OnRide", c => c.Boolean(nullable: false));
            AddColumn("dbo.Drivers", "OnLine", c => c.Boolean(nullable: false));
        }
    }
}