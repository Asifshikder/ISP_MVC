namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentPaidByAndDeleteByParentInPurchasePaymentHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchasePaymentHistory", "PaymentPaidBy", c => c.Int(nullable: false));
            AddColumn("dbo.PurchasePaymentHistory", "DeleteByParent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchasePaymentHistory", "DeleteByParent");
            DropColumn("dbo.PurchasePaymentHistory", "PaymentPaidBy");
        }
    }
}
