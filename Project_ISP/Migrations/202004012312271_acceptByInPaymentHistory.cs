namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class acceptByInPaymentHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentHistories", "BillAcceptBy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentHistories", "BillAcceptBy");
        }
    }
}
