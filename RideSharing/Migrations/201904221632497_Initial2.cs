namespace RideSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "DriverIdentity", c => c.String());
            AddColumn("dbo.Trips", "PassengerIdentity", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Trips", "PassengerIdentity");
            DropColumn("dbo.Trips", "DriverIdentity");
        }
    }
}
