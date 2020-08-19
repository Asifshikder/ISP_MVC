namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YearMonthDayAddedInAcountHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingHistory", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.AccountingHistory", "Month", c => c.Int(nullable: false));
            AddColumn("dbo.AccountingHistory", "Day", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountingHistory", "Day");
            DropColumn("dbo.AccountingHistory", "Month");
            DropColumn("dbo.AccountingHistory", "Year");
        }
    }
}
