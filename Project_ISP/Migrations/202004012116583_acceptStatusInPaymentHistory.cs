namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class acceptStatusInPaymentHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentHistories", "AcceptStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentHistories", "AcceptStatus");
        }
    }
}
