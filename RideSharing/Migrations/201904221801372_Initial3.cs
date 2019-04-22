namespace RideSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Trips", "Total");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trips", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
