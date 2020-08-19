namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientDetailsLatitudeLogitude : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientDetails", "LatitudeLongitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientDetails", "LatitudeLongitude");
        }
    }
}
