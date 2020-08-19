namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentfromwhichpageinpaymenthistorytableInstring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentHistories", "PaymentFromWhichPage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentHistories", "PaymentFromWhichPage", c => c.Int(nullable: false));
        }
    }
}
