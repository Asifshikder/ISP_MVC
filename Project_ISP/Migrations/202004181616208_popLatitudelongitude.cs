namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class popLatitudelongitude : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pops", "LatitudeLongitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pops", "LatitudeLongitude");
        }
    }
}
