namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLatitudeLognitude : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientDetails", "LatitudeLongitude", c => c.String());
            AddColumn("dbo.Boxes", "LatitudeLongitude", c => c.String());
            AddColumn("dbo.Pops", "LatitudeLongitude", c => c.String());
            DropColumn("dbo.ClientDetails", "Latitude_Longitude");
            DropColumn("dbo.Boxes", "Latitude_Longitude");
            DropColumn("dbo.Pops", "Latitude_Longitude");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pops", "Latitude_Longitude", c => c.String());
            AddColumn("dbo.Boxes", "Latitude_Longitude", c => c.String());
            AddColumn("dbo.ClientDetails", "Latitude_Longitude", c => c.String());
            DropColumn("dbo.Pops", "LatitudeLongitude");
            DropColumn("dbo.Boxes", "LatitudeLongitude");
            DropColumn("dbo.ClientDetails", "LatitudeLongitude");
        }
    }
}
