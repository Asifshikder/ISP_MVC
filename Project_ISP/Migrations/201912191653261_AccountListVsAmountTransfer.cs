namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountListVsAmountTransfer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountListVsAmountTransfers",
                c => new
                    {
                        AccountListVsAmountTransferID = c.Int(nullable: false, identity: true),
                        AccountListID = c.Int(nullable: false),
                        ToAccountID = c.Int(nullable: false),
                        TransferDate = c.DateTime(nullable: false),
                        CurrencyID = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        Tags = c.String(),
                        PaymentByID = c.Int(nullable: false),
                        References = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccountListVsAmountTransferID)
                .ForeignKey("dbo.AccountLists", t => t.AccountListID)
                .ForeignKey("dbo.PaymentBies", t => t.PaymentByID)
                .Index(t => t.AccountListID)
                .Index(t => t.PaymentByID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountListVsAmountTransfers", "PaymentByID", "dbo.PaymentBies");
            DropForeignKey("dbo.AccountListVsAmountTransfers", "AccountListID", "dbo.AccountLists");
            DropIndex("dbo.AccountListVsAmountTransfers", new[] { "PaymentByID" });
            DropIndex("dbo.AccountListVsAmountTransfers", new[] { "AccountListID" });
            DropTable("dbo.AccountListVsAmountTransfers");
        }
    }
}
