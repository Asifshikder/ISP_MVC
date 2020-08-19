namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyAndCompanyPersonAndDepositAndExpenseAndUnknownHead : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Expenses", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.Expenses", new[] { "EmployeeID" });
            DropIndex("dbo.Expenses", new[] { "ResellerID" });
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        CompanyEmail = c.String(),
                        CompanyAddress = c.String(),
                        ContactPerson = c.String(),
                        Phone = c.String(),
                        CompanyLogo = c.Binary(),
                        CompanyLogoPath = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo.CompanyVSPayers",
                c => new
                    {
                        PayerID = c.Int(nullable: false, identity: true),
                        PayerName = c.String(),
                        CompanyID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PayerID)
                .ForeignKey("dbo.Companies", t => t.CompanyID)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.Deposits",
                c => new
                    {
                        DepositID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        DescriptionFileByte = c.Binary(),
                        DescriptionFilePath = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HeadID = c.Int(nullable: false),
                        DepositDate = c.DateTime(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        AccountListID = c.Int(nullable: false),
                        PayerID = c.Int(nullable: false),
                        PaymentByID = c.Int(nullable: false),
                        DepositStatus = c.Int(nullable: false),
                        References = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DepositID)
                .ForeignKey("dbo.AccountLists", t => t.AccountListID)
                .ForeignKey("dbo.Companies", t => t.CompanyID)
                .ForeignKey("dbo.CompanyVSPayers", t => t.PayerID)
                .ForeignKey("dbo.Heads", t => t.HeadID)
                .ForeignKey("dbo.PaymentBies", t => t.PaymentByID)
                .Index(t => t.HeadID)
                .Index(t => t.CompanyID)
                .Index(t => t.AccountListID)
                .Index(t => t.PayerID)
                .Index(t => t.PaymentByID);
            
            CreateTable(
                "dbo.Heads",
                c => new
                    {
                        HeadID = c.Int(nullable: false, identity: true),
                        HeadeName = c.String(),
                        HeadTypeID = c.Int(nullable: false),
                        ResellerID = c.Int(),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HeadID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ResellerID);
            
            AddColumn("dbo.Expenses", "Descriptions", c => c.String());
            AddColumn("dbo.Expenses", "DescriptionFileByte", c => c.Binary());
            AddColumn("dbo.Expenses", "DescriptionFilePath", c => c.String());
            AddColumn("dbo.Expenses", "HeadID", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "CompanyID", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "AccountListID", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "PayerID", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "PaymentByID", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "ExpenseStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "References", c => c.String());
            AddColumn("dbo.Expenses", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "CreateBy", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Expenses", "DeleteBy", c => c.Int());
            AddColumn("dbo.Expenses", "DeleteDate", c => c.DateTime());
            AlterColumn("dbo.Expenses", "ResellerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Expenses", "HeadID");
            CreateIndex("dbo.Expenses", "CompanyID");
            CreateIndex("dbo.Expenses", "AccountListID");
            CreateIndex("dbo.Expenses", "PayerID");
            CreateIndex("dbo.Expenses", "PaymentByID");
            CreateIndex("dbo.Expenses", "ResellerID");
            AddForeignKey("dbo.Expenses", "AccountListID", "dbo.AccountLists", "AccountListID");
            AddForeignKey("dbo.Expenses", "CompanyID", "dbo.Companies", "CompanyID");
            AddForeignKey("dbo.Expenses", "PayerID", "dbo.CompanyVSPayers", "PayerID");
            AddForeignKey("dbo.Expenses", "HeadID", "dbo.Heads", "HeadID");
            AddForeignKey("dbo.Expenses", "PaymentByID", "dbo.PaymentBies", "PaymentByID");
            DropColumn("dbo.Expenses", "Subject");
            DropColumn("dbo.Expenses", "Details");
            DropColumn("dbo.Expenses", "PaidTo");
            DropColumn("dbo.Expenses", "EmployeeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "EmployeeID", c => c.Int());
            AddColumn("dbo.Expenses", "PaidTo", c => c.String());
            AddColumn("dbo.Expenses", "Details", c => c.String());
            AddColumn("dbo.Expenses", "Subject", c => c.String());
            DropForeignKey("dbo.Expenses", "PaymentByID", "dbo.PaymentBies");
            DropForeignKey("dbo.Expenses", "HeadID", "dbo.Heads");
            DropForeignKey("dbo.Expenses", "PayerID", "dbo.CompanyVSPayers");
            DropForeignKey("dbo.Expenses", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Expenses", "AccountListID", "dbo.AccountLists");
            DropForeignKey("dbo.Deposits", "PaymentByID", "dbo.PaymentBies");
            DropForeignKey("dbo.Deposits", "HeadID", "dbo.Heads");
            DropForeignKey("dbo.Heads", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.Deposits", "PayerID", "dbo.CompanyVSPayers");
            DropForeignKey("dbo.Deposits", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Deposits", "AccountListID", "dbo.AccountLists");
            DropForeignKey("dbo.CompanyVSPayers", "CompanyID", "dbo.Companies");
            DropIndex("dbo.Expenses", new[] { "ResellerID" });
            DropIndex("dbo.Expenses", new[] { "PaymentByID" });
            DropIndex("dbo.Expenses", new[] { "PayerID" });
            DropIndex("dbo.Expenses", new[] { "AccountListID" });
            DropIndex("dbo.Expenses", new[] { "CompanyID" });
            DropIndex("dbo.Expenses", new[] { "HeadID" });
            DropIndex("dbo.Heads", new[] { "ResellerID" });
            DropIndex("dbo.Deposits", new[] { "PaymentByID" });
            DropIndex("dbo.Deposits", new[] { "PayerID" });
            DropIndex("dbo.Deposits", new[] { "AccountListID" });
            DropIndex("dbo.Deposits", new[] { "CompanyID" });
            DropIndex("dbo.Deposits", new[] { "HeadID" });
            DropIndex("dbo.CompanyVSPayers", new[] { "CompanyID" });
            AlterColumn("dbo.Expenses", "ResellerID", c => c.Int());
            DropColumn("dbo.Expenses", "DeleteDate");
            DropColumn("dbo.Expenses", "DeleteBy");
            DropColumn("dbo.Expenses", "CreateDate");
            DropColumn("dbo.Expenses", "CreateBy");
            DropColumn("dbo.Expenses", "Status");
            DropColumn("dbo.Expenses", "References");
            DropColumn("dbo.Expenses", "ExpenseStatus");
            DropColumn("dbo.Expenses", "PaymentByID");
            DropColumn("dbo.Expenses", "PayerID");
            DropColumn("dbo.Expenses", "AccountListID");
            DropColumn("dbo.Expenses", "CompanyID");
            DropColumn("dbo.Expenses", "HeadID");
            DropColumn("dbo.Expenses", "DescriptionFilePath");
            DropColumn("dbo.Expenses", "DescriptionFileByte");
            DropColumn("dbo.Expenses", "Descriptions");
            DropTable("dbo.Heads");
            DropTable("dbo.Deposits");
            DropTable("dbo.CompanyVSPayers");
            DropTable("dbo.Companies");
            CreateIndex("dbo.Expenses", "ResellerID");
            CreateIndex("dbo.Expenses", "EmployeeID");
            AddForeignKey("dbo.Expenses", "EmployeeID", "dbo.Employees", "EmployeeID");
        }
    }
}
