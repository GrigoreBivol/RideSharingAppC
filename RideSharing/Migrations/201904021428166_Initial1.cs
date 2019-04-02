namespace RideSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "DriverIdentity", c => c.String());
            AddColumn("dbo.Passengers", "PassengerIdentity", c => c.String());
            AddColumn("dbo.Passengers", "Name", c => c.String());
            DropColumn("dbo.Passengers", "FirstName");
            DropColumn("dbo.Passengers", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Passengers", "LastName", c => c.String());
            AddColumn("dbo.Passengers", "FirstName", c => c.String());
            DropColumn("dbo.Passengers", "Name");
            DropColumn("dbo.Passengers", "PassengerIdentity");
            DropColumn("dbo.Drivers", "DriverIdentity");
        }
    }
}
