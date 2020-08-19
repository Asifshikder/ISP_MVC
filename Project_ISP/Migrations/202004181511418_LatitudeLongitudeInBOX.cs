namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatitudeLongitudeInBOX : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boxes", "LatitudeLongitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boxes", "LatitudeLongitude");
        }
    }
}
