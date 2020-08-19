namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentbyChangeFromPaymentByToPaymentMethodEnum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PurchasePaymentHistory", "PaymentByID", "dbo.PaymentBies");
            DropIndex("dbo.PurchasePaymentHistory", new[] { "PaymentByID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.PurchasePaymentHistory", "PaymentByID");
            AddForeignKey("dbo.PurchasePaymentHistory", "PaymentByID", "dbo.PaymentBies", "PaymentByID");
        }
    }
}
