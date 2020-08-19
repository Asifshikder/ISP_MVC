namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InforamtionInPurchasePaymentHistoryTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PurchasePaymentHistories", newName: "PurchasePaymentHistory");
            AddColumn("dbo.PurchasePaymentHistory", "AccountListID", c => c.Int(nullable: false));
            AddColumn("dbo.PurchasePaymentHistory", "PaymentByID", c => c.Int(nullable: false));
            AddColumn("dbo.PurchasePaymentHistory", "PurchasePaymentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PurchasePaymentHistory", "CheckNo", c => c.String());
            AddColumn("dbo.PurchasePaymentHistory", "CheckName", c => c.String());
            AddColumn("dbo.PurchasePaymentHistory", "CheckPath", c => c.String());
            AddColumn("dbo.PurchasePaymentHistory", "CheckImageBytes", c => c.Binary());
            AddColumn("dbo.PurchasePaymentHistory", "Description", c => c.String());
            AddColumn("dbo.PurchasePaymentHistory", "CreateBy", c => c.Int(nullable: false));
            AddColumn("dbo.PurchasePaymentHistory", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PurchasePaymentHistory", "UpdateBy", c => c.Int());
            AddColumn("dbo.PurchasePaymentHistory", "UpdateDate", c => c.DateTime());
            AddColumn("dbo.PurchasePaymentHistory", "DeleteBy", c => c.Int());
            AddColumn("dbo.PurchasePaymentHistory", "DeleteDate", c => c.DateTime());
            CreateIndex("dbo.PurchasePaymentHistory", "AccountListID");
            CreateIndex("dbo.PurchasePaymentHistory", "PaymentByID");
            AddForeignKey("dbo.PurchasePaymentHistory", "AccountListID", "dbo.AccountLists", "AccountListID");
            AddForeignKey("dbo.PurchasePaymentHistory", "PaymentByID", "dbo.PaymentBies", "PaymentByID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchasePaymentHistory", "PaymentByID", "dbo.PaymentBies");
            DropForeignKey("dbo.PurchasePaymentHistory", "AccountListID", "dbo.AccountLists");
            DropIndex("dbo.PurchasePaymentHistory", new[] { "PaymentByID" });
            DropIndex("dbo.PurchasePaymentHistory", new[] { "AccountListID" });
            DropColumn("dbo.PurchasePaymentHistory", "DeleteDate");
            DropColumn("dbo.PurchasePaymentHistory", "DeleteBy");
            DropColumn("dbo.PurchasePaymentHistory", "UpdateDate");
            DropColumn("dbo.PurchasePaymentHistory", "UpdateBy");
            DropColumn("dbo.PurchasePaymentHistory", "CreateDate");
            DropColumn("dbo.PurchasePaymentHistory", "CreateBy");
            DropColumn("dbo.PurchasePaymentHistory", "Description");
            DropColumn("dbo.PurchasePaymentHistory", "CheckImageBytes");
            DropColumn("dbo.PurchasePaymentHistory", "CheckPath");
            DropColumn("dbo.PurchasePaymentHistory", "CheckName");
            DropColumn("dbo.PurchasePaymentHistory", "CheckNo");
            DropColumn("dbo.PurchasePaymentHistory", "PurchasePaymentDate");
            DropColumn("dbo.PurchasePaymentHistory", "PaymentByID");
            DropColumn("dbo.PurchasePaymentHistory", "AccountListID");
            RenameTable(name: "dbo.PurchasePaymentHistory", newName: "PurchasePaymentHistories");
        }
    }
}
