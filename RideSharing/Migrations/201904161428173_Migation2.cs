namespace RideSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migation2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Trips", "Passenger_Id", "dbo.Passengers");
            RenameColumn(table: "dbo.Trips", name: "Passenger_Id", newName: "Passenger_PassengerId");
            RenameIndex(table: "dbo.Trips", name: "IX_Passenger_Id", newName: "IX_Passenger_PassengerId");
            DropPrimaryKey("dbo.Passengers");
            AddColumn("dbo.Passengers", "PassengerId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Passengers", "OnLine", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.Passengers", "PassengerId");
            AddForeignKey("dbo.Trips", "Passenger_PassengerId", "dbo.Passengers", "PassengerId");
            DropColumn("dbo.Passengers", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Passengers", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Trips", "Passenger_PassengerId", "dbo.Passengers");
            DropPrimaryKey("dbo.Passengers");
            DropColumn("dbo.Passengers", "OnLine");
            DropColumn("dbo.Passengers", "PassengerId");
            AddPrimaryKey("dbo.Passengers", "Id");
            RenameIndex(table: "dbo.Trips", name: "IX_Passenger_PassengerId", newName: "IX_Passenger_Id");
            RenameColumn(table: "dbo.Trips", name: "Passenger_PassengerId", newName: "Passenger_Id");
            AddForeignKey("dbo.Trips", "Passenger_Id", "dbo.Passengers", "Id");
        }
    }
}
