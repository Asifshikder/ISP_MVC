namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFromAccountInaccountlistbsamounttransfer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountListVsAmountTransfers", "AccountListID", "dbo.AccountLists");
            DropIndex("dbo.AccountListVsAmountTransfers", new[] { "AccountListID" });
            AddColumn("dbo.AccountListVsAmountTransfers", "FromAccountID", c => c.Int(nullable: false));
            AddColumn("dbo.AccountListVsAmountTransfers", "BreakDownAccountListID", c => c.Int(nullable: false));
            AddColumn("dbo.AccountListVsAmountTransfers", "TransferType", c => c.String());
            DropColumn("dbo.AccountListVsAmountTransfers", "AccountListID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AccountListVsAmountTransfers", "AccountListID", c => c.Int(nullable: false));
            DropColumn("dbo.AccountListVsAmountTransfers", "TransferType");
            DropColumn("dbo.AccountListVsAmountTransfers", "BreakDownAccountListID");
            DropColumn("dbo.AccountListVsAmountTransfers", "FromAccountID");
            CreateIndex("dbo.AccountListVsAmountTransfers", "AccountListID");
            AddForeignKey("dbo.AccountListVsAmountTransfers", "AccountListID", "dbo.AccountLists", "AccountListID");
        }
    }
}
