namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountListInAccountingHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingHistory", "AccountListID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountingHistory", "AccountListID");
        }
    }
}
