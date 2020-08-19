namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountingHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountingHistory",
                c => new
                    {
                        AccountingHistoryID = c.Int(nullable: false, identity: true),
                        PurchaseID = c.Int(nullable: false),
                        SalesID = c.Int(nullable: false),
                        DepositID = c.Int(nullable: false),
                        ExpenseID = c.Int(nullable: false),
                        ActionTypeID = c.Int(nullable: false),
                        DRCRTypeID = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AccountingHistoryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccountingHistory");
        }
    }
}
