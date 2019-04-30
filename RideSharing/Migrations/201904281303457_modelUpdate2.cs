namespace RideSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelUpdate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Trips", "IsCompleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trips", "IsCompleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Trips", "Status");
        }
    }
}
