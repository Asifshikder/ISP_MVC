namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentfromwhichpageinpaymenthistorytable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentHistories", "PaymentFromWhichPage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentHistories", "PaymentFromWhichPage");
        }
    }
}
