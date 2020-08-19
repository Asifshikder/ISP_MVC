namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateByDateetc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingHistory", "CreateBy", c => c.Int(nullable: false));
            AddColumn("dbo.AccountingHistory", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AccountingHistory", "UpdateBy", c => c.Int(nullable: false));
            AddColumn("dbo.AccountingHistory", "UpdateDate", c => c.DateTime());
            AddColumn("dbo.AccountingHistory", "DeleteBy", c => c.Int(nullable: false));
            AddColumn("dbo.AccountingHistory", "DeteDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountingHistory", "DeteDate");
            DropColumn("dbo.AccountingHistory", "DeleteBy");
            DropColumn("dbo.AccountingHistory", "UpdateDate");
            DropColumn("dbo.AccountingHistory", "UpdateBy");
            DropColumn("dbo.AccountingHistory", "CreateDate");
            DropColumn("dbo.AccountingHistory", "CreateBy");
        }
    }
}
