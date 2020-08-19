namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentHistoryAndAccountOwnerAndAccountList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountLists",
                c => new
                    {
                        AccountListID = c.Int(nullable: false, identity: true),
                        AccountTitle = c.String(),
                        Description = c.String(),
                        InitialBalance = c.Decimal(precision: 18, scale: 2),
                        AccountNumber = c.Int(nullable: false),
                        ContactPerson = c.String(),
                        Phone = c.String(),
                        BankUrl = c.String(),
                        OwnerID = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateData = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccountListID)
                .ForeignKey("dbo.AccountOwners", t => t.OwnerID)
                .Index(t => t.OwnerID);
            
            CreateTable(
                "dbo.AccountOwners",
                c => new
                    {
                        OwnerID = c.Int(nullable: false, identity: true),
                        OwnerName = c.String(),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OwnerID);
            
            CreateTable(
                "dbo.PurchasePaymentHistories",
                c => new
                    {
                        PurchasePaymentHistoryID = c.Int(nullable: false, identity: true),
                        PurchaseID = c.Int(nullable: false),
                        PaymentAmount = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchasePaymentHistoryID)
                .ForeignKey("dbo.Purchase", t => t.PurchaseID)
                .Index(t => t.PurchaseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchasePaymentHistories", "PurchaseID", "dbo.Purchase");
            DropForeignKey("dbo.AccountLists", "OwnerID", "dbo.AccountOwners");
            DropIndex("dbo.PurchasePaymentHistories", new[] { "PurchaseID" });
            DropIndex("dbo.AccountLists", new[] { "OwnerID" });
            DropTable("dbo.PurchasePaymentHistories");
            DropTable("dbo.AccountOwners");
            DropTable("dbo.AccountLists");
        }
    }
}
