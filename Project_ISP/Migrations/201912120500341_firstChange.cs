namespace Project_ISP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionLists",
                c => new
                    {
                        ActionListID = c.Int(nullable: false, identity: true),
                        FormID = c.Int(nullable: false),
                        ActionName = c.String(),
                        ActionValue = c.String(),
                        ActionDescription = c.String(),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        ShowingStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionListID)
                .ForeignKey("dbo.Forms", t => t.FormID)
                .Index(t => t.FormID);
            
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        FormID = c.Int(nullable: false, identity: true),
                        ControllerNameID = c.Int(nullable: false),
                        FormName = c.String(),
                        FormValue = c.String(),
                        FormDescription = c.String(),
                        FormLocation = c.String(),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        ShowingStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FormID)
                .ForeignKey("dbo.ControllerNames", t => t.ControllerNameID)
                .Index(t => t.ControllerNameID);
            
            CreateTable(
                "dbo.ControllerNames",
                c => new
                    {
                        ControllerNameID = c.Int(nullable: false, identity: true),
                        ControllerNames = c.String(),
                        ControllerValue = c.String(),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        ShowingStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ControllerNameID);
            
            CreateTable(
                "dbo.AdvancePayments",
                c => new
                    {
                        AdvancePaymentID = c.Int(nullable: false, identity: true),
                        ClientDetailsID = c.Int(nullable: false),
                        AdvanceAmount = c.Double(nullable: false),
                        Remarks = c.String(),
                        CollectBy = c.String(),
                        CreatePaymentBy = c.String(),
                        FirstPaymentDate = c.DateTime(nullable: false),
                        UpdatePaymentBy = c.String(),
                        UpdatePaymentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AdvancePaymentID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .Index(t => t.ClientDetailsID);
            
            CreateTable(
                "dbo.ClientDetails",
                c => new
                    {
                        ClientDetailsID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LoginName = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        ContactNumber = c.String(),
                        Occupation = c.String(),
                        SocialCommunicationURL = c.String(),
                        Remarks = c.String(),
                        BoxNumber = c.String(),
                        PopDetails = c.String(),
                        RequireCable = c.String(),
                        Reference = c.String(),
                        NationalID = c.String(),
                        ConnectionFeesProvidedDate = c.DateTime(),
                        ClientSurvey = c.String(),
                        ConnectionDate = c.DateTime(),
                        ClientPriority = c.Int(),
                        MacAddress = c.String(),
                        SMSCommunication = c.String(),
                        IsNewClient = c.Int(),
                        NewClientRequestDate = c.DateTime(),
                        NewClientApproximateConnectionStartDate = c.DateTime(),
                        ZoneID = c.Int(),
                        ConnectionTypeID = c.Int(),
                        CableTypeID = c.Int(),
                        PackageID = c.Int(),
                        SecurityQuestionID = c.Int(),
                        SecurityQuestionAnswer = c.String(),
                        EmployeeID = c.Int(),
                        RoleID = c.Int(),
                        UserRightPermissionID = c.Int(),
                        MikrotikID = c.Int(),
                        IP = c.String(),
                        Mac = c.String(),
                        ResellerID = c.Int(),
                        IsPriorityClient = c.Boolean(nullable: false),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        ApproxPaymentDate = c.Int(nullable: false),
                        ProfileUpdatePercentage = c.Double(nullable: false),
                        StatusThisMonth = c.Int(nullable: false),
                        StatusNextMonth = c.Int(nullable: false),
                        LineStatusWillActiveInThisDate = c.DateTime(nullable: false),
                        ClientOwnImageBytes = c.Binary(),
                        ClientOwnImageBytesPaths = c.String(),
                        ClientNIDImageBytes = c.Binary(),
                        ClientNIDImageBytesPaths = c.String(),
                        PackageThisMonth = c.Int(nullable: false),
                        PackageNextMonth = c.Int(nullable: false),
                        NextApproxPaymentFullDate = c.DateTime(),
                        RunningCycle = c.String(),
                        ClientLineStatusID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        PermanentDiscount = c.Double(nullable: false),
                        CableUnit_CableUnitID = c.Int(),
                    })
                .PrimaryKey(t => t.ClientDetailsID)
                .ForeignKey("dbo.CableTypes", t => t.CableTypeID)
                .ForeignKey("dbo.ConnectionTypes", t => t.ConnectionTypeID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Mikrotiks", t => t.MikrotikID)
                .ForeignKey("dbo.Packages", t => t.PackageID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .ForeignKey("dbo.SecurityQuestions", t => t.SecurityQuestionID)
                .ForeignKey("dbo.UserRightPermissions", t => t.UserRightPermissionID)
                .ForeignKey("dbo.Zones", t => t.ZoneID)
                .ForeignKey("dbo.CableUnits", t => t.CableUnit_CableUnitID)
                .Index(t => t.ZoneID)
                .Index(t => t.ConnectionTypeID)
                .Index(t => t.CableTypeID)
                .Index(t => t.PackageID)
                .Index(t => t.SecurityQuestionID)
                .Index(t => t.EmployeeID)
                .Index(t => t.RoleID)
                .Index(t => t.UserRightPermissionID)
                .Index(t => t.MikrotikID)
                .Index(t => t.ResellerID)
                .Index(t => t.CableUnit_CableUnitID);
            
            CreateTable(
                "dbo.CableTypes",
                c => new
                    {
                        CableTypeID = c.Int(nullable: false, identity: true),
                        CableTypeName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CableTypeID);
            
            CreateTable(
                "dbo.ConnectionTypes",
                c => new
                    {
                        ConnectionTypeID = c.Int(nullable: false, identity: true),
                        ConnectionTypeName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ConnectionTypeID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LoginName = c.String(),
                        Password = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        DepartmentID = c.Int(),
                        RoleID = c.Int(),
                        EmployeeStatus = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        UserRightPermissionID = c.Int(),
                        DOB = c.DateTime(nullable: false),
                        DeviceID = c.Int(nullable: false),
                        DutyShiftID = c.Int(nullable: false),
                        Salary = c.Int(nullable: false),
                        DayID = c.Int(),
                        BreakHour = c.Int(nullable: false),
                        BreakMinute = c.Int(nullable: false),
                        DutyShiftCombined = c.String(),
                        EmployeeOwnImageBytes = c.Binary(),
                        EmployeeOwnImageBytesPaths = c.String(),
                        EmployeeNIDImageBytes = c.Binary(),
                        EmployeeNIDImageBytesPaths = c.String(),
                        ResellerID = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Days", t => t.DayID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .ForeignKey("dbo.DutyShifts", t => t.DutyShiftID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .ForeignKey("dbo.UserRightPermissions", t => t.UserRightPermissionID)
                .Index(t => t.DepartmentID)
                .Index(t => t.RoleID)
                .Index(t => t.UserRightPermissionID)
                .Index(t => t.DutyShiftID)
                .Index(t => t.DayID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        DayID = c.Int(nullable: false, identity: true),
                        DayName = c.String(),
                    })
                .PrimaryKey(t => t.DayID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.DutyShifts",
                c => new
                    {
                        DutyShiftID = c.Int(nullable: false, identity: true),
                        StartHour = c.Int(nullable: false),
                        StartMinute = c.Int(nullable: false),
                        EndHour = c.Int(nullable: false),
                        EndMinute = c.Int(nullable: false),
                        TableStatusID = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateTime = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        CombineDutyShifts = c.String(),
                    })
                .PrimaryKey(t => t.DutyShiftID);
            
            CreateTable(
                "dbo.Resellers",
                c => new
                    {
                        ResellerID = c.Int(nullable: false, identity: true),
                        ResellerName = c.String(),
                        ResellerLoginName = c.String(),
                        ResellerBusinessName = c.String(),
                        ResellerPassword = c.String(),
                        ResellerTypeListID = c.String(),
                        ResellerAddress = c.String(),
                        ResellerContact = c.String(),
                        ResellerBillingCycleList = c.String(),
                        ResellerStatus = c.Int(nullable: false),
                        ResellerLogo = c.Binary(),
                        ResellerLogoPath = c.String(),
                        BandwithReselleItemGivenWithPrice = c.String(),
                        macReselleGivenPackageWithPrice = c.String(),
                        ResellerBalance = c.Double(nullable: false),
                        RoleID = c.Int(),
                        UserRightPermissionID = c.Int(),
                        MacResellerAssignMikrotik = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ResellerID)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .ForeignKey("dbo.UserRightPermissions", t => t.UserRightPermissionID)
                .Index(t => t.RoleID)
                .Index(t => t.UserRightPermissionID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleNae = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.UserRightPermissions",
                c => new
                    {
                        UserRightPermissionID = c.Int(nullable: false, identity: true),
                        UserRightPermissionName = c.String(),
                        UserRightPermissionDescription = c.String(),
                        UserRightPermissionDetails = c.String(),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserRightPermissionID);
            
            CreateTable(
                "dbo.Mikrotiks",
                c => new
                    {
                        MikrotikID = c.Int(nullable: false, identity: true),
                        MikName = c.String(),
                        RealIP = c.String(),
                        MikUserName = c.String(),
                        MikPassword = c.String(),
                        APIPort = c.Int(nullable: false),
                        WebPort = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.Int(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MikrotikID);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PackageID = c.Int(nullable: false, identity: true),
                        IPPoolID = c.Int(),
                        MikrotikID = c.Int(),
                        LocalAddress = c.String(),
                        PackageName = c.String(),
                        OldPackageName = c.String(),
                        PackagePrice = c.Single(nullable: false),
                        BandWith = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        PackageForMyOrResellerUser = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PackageID)
                .ForeignKey("dbo.IPPools", t => t.IPPoolID)
                .ForeignKey("dbo.Mikrotiks", t => t.MikrotikID)
                .Index(t => t.IPPoolID)
                .Index(t => t.MikrotikID);
            
            CreateTable(
                "dbo.IPPools",
                c => new
                    {
                        IPPoolID = c.Int(nullable: false, identity: true),
                        PoolName = c.String(),
                        StartRange = c.String(),
                        EndRange = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.Int(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.IPPoolID);
            
            CreateTable(
                "dbo.SecurityQuestions",
                c => new
                    {
                        SecurityQuestionID = c.Int(nullable: false, identity: true),
                        SecurityQuestionName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SecurityQuestionID);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        ZoneID = c.Int(nullable: false, identity: true),
                        ZoneName = c.String(),
                        ResellerID = c.Int(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ZoneID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        AssetID = c.Int(nullable: false, identity: true),
                        AssetTypeID = c.Int(nullable: false),
                        AssetName = c.String(),
                        AssetValue = c.Double(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        SerialNumber = c.String(),
                        WarrentyStartDate = c.DateTime(),
                        WarrentyEndDate = c.DateTime(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AssetID)
                .ForeignKey("dbo.AssetTypes", t => t.AssetTypeID)
                .Index(t => t.AssetTypeID);
            
            CreateTable(
                "dbo.AssetTypes",
                c => new
                    {
                        AssetTypeID = c.Int(nullable: false, identity: true),
                        AssetTypeName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AssetTypeID);
            
            CreateTable(
                "dbo.AtendaceInOuts",
                c => new
                    {
                        AttendaceInOutID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        start = c.Int(nullable: false),
                        end = c.Int(nullable: false),
                        InSalaryCut = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OutSalaryCut = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AttendanceTypeID = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttendaceInOutID)
                .ForeignKey("dbo.AttendanceTypes", t => t.AttendanceTypeID)
                .Index(t => t.AttendanceTypeID);
            
            CreateTable(
                "dbo.AttendanceTypes",
                c => new
                    {
                        AttendanceTypeID = c.Int(nullable: false, identity: true),
                        AttendanceName = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AttendanceTypeID);
            
            CreateTable(
                "dbo.BandwithResellerGivenItems",
                c => new
                    {
                        BandwithResellerGivenItemID = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        MeasureUnit = c.String(),
                        PerUnitCommonPrice = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BandwithResellerGivenItemID);
            
            CreateTable(
                "dbo.BannedStatus",
                c => new
                    {
                        BannedStatusID = c.Int(nullable: false, identity: true),
                        BannedStatusName = c.String(),
                    })
                .PrimaryKey(t => t.BannedStatusID);
            
            CreateTable(
                "dbo.BillGenerateHistories",
                c => new
                    {
                        BIllGenerateHistoryID = c.Int(nullable: false, identity: true),
                        Year = c.String(),
                        Month = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BIllGenerateHistoryID);
            
            CreateTable(
                "dbo.Boxes",
                c => new
                    {
                        BoxID = c.Int(nullable: false, identity: true),
                        BoxName = c.String(),
                        ResellerID = c.Int(),
                        BoxLocation = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BoxID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        BrandID = c.Int(nullable: false, identity: true),
                        BrandName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BrandID);
            
            CreateTable(
                "dbo.CableDistributions",
                c => new
                    {
                        CableDistributionID = c.Int(nullable: false, identity: true),
                        ClientDetailsID = c.Int(),
                        EmployeeID = c.Int(),
                        CableForEmployeeID = c.Int(),
                        CableStockID = c.Int(nullable: false),
                        AmountOfCableUsed = c.Int(nullable: false),
                        Purpose = c.String(),
                        CableAssignFromWhere = c.Int(nullable: false),
                        CableIndicatorStatus = c.Int(nullable: false),
                        Remarks = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CableDistributionID)
                .ForeignKey("dbo.CableStocks", t => t.CableStockID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .Index(t => t.ClientDetailsID)
                .Index(t => t.EmployeeID)
                .Index(t => t.CableStockID);
            
            CreateTable(
                "dbo.CableStocks",
                c => new
                    {
                        CableStockID = c.Int(nullable: false, identity: true),
                        CableTypeID = c.Int(nullable: false),
                        BrandID = c.Int(),
                        SupplierID = c.Int(),
                        SupplierInvoice = c.String(),
                        FromReading = c.Int(nullable: false),
                        ToReading = c.Int(nullable: false),
                        CableUnitID = c.Int(nullable: false),
                        CableBoxName = c.String(),
                        CableQuantity = c.Int(nullable: false),
                        UsedQuantityFromThisBox = c.Int(nullable: false),
                        TotallyUsed = c.Int(),
                        EmployeeID = c.Int(nullable: false),
                        IndicatorStatus = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CableStockID)
                .ForeignKey("dbo.Brands", t => t.BrandID)
                .ForeignKey("dbo.CableTypes", t => t.CableTypeID)
                .ForeignKey("dbo.CableUnits", t => t.CableUnitID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID)
                .Index(t => t.CableTypeID)
                .Index(t => t.BrandID)
                .Index(t => t.SupplierID)
                .Index(t => t.CableUnitID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.CableUnits",
                c => new
                    {
                        CableUnitID = c.Int(nullable: false, identity: true),
                        CableUnitName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CableUnitID);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(),
                        SupplierAddress = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SupplierID);
            
            CreateTable(
                "dbo.ClientBannedStatus",
                c => new
                    {
                        ClientBannedStatusID = c.Int(nullable: false, identity: true),
                        ClientDetailsID = c.Int(nullable: false),
                        BannedStatusID = c.Int(nullable: false),
                        BannedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClientBannedStatusID)
                .ForeignKey("dbo.BannedStatus", t => t.BannedStatusID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .Index(t => t.ClientDetailsID)
                .Index(t => t.BannedStatusID);
            
            CreateTable(
                "dbo.ClientDueBills",
                c => new
                    {
                        ClientDueBillsID = c.Int(nullable: false, identity: true),
                        ClientDetailsID = c.Int(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientDueBillsID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .Index(t => t.ClientDetailsID);
            
            CreateTable(
                "dbo.ClientLineStatus",
                c => new
                    {
                        ClientLineStatusID = c.Int(nullable: false, identity: true),
                        ClientDetailsID = c.Int(nullable: false),
                        PackageID = c.Int(),
                        LineStatusID = c.Int(nullable: false),
                        LineStatusFromWhichMonth = c.Int(),
                        StatusChangeReason = c.String(),
                        LineStatusChangeDate = c.DateTime(),
                        EmployeeID = c.Int(),
                        ResellerID = c.Int(),
                        CreateDate = c.DateTime(),
                        MikrotikID = c.Int(),
                        IsLineStatusApplied = c.Boolean(nullable: false),
                        LineStatusWillActiveInThisDate = c.DateTime(),
                        StatusFromNow = c.Boolean(nullable: false),
                        StatusThisMonth = c.Int(nullable: false),
                        StatusNextMonth = c.Int(nullable: false),
                        PackageThisMonth = c.Int(nullable: false),
                        PackageNextMonth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientLineStatusID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.LineStatus", t => t.LineStatusID)
                .ForeignKey("dbo.Mikrotiks", t => t.MikrotikID)
                .ForeignKey("dbo.Packages", t => t.PackageID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ClientDetailsID)
                .Index(t => t.PackageID)
                .Index(t => t.LineStatusID)
                .Index(t => t.EmployeeID)
                .Index(t => t.ResellerID)
                .Index(t => t.MikrotikID);
            
            CreateTable(
                "dbo.LineStatus",
                c => new
                    {
                        LineStatusID = c.Int(nullable: false, identity: true),
                        LineStatusName = c.String(),
                    })
                .PrimaryKey(t => t.LineStatusID);
            
            CreateTable(
                "dbo.Complains",
                c => new
                    {
                        ComplainID = c.Int(nullable: false, identity: true),
                        ClientDetailsID = c.Int(nullable: false),
                        TokenNo = c.Int(nullable: false),
                        MonthlySerialNo = c.String(),
                        ComplainDetails = c.String(),
                        EmployeeID = c.Int(),
                        ResellerID = c.Int(),
                        LineStatusID = c.Int(nullable: false),
                        ComplainTypeID = c.Int(nullable: false),
                        WhichOrWhere = c.String(),
                        ComplainOpenBy = c.Int(nullable: false),
                        ComplainTime = c.DateTime(nullable: false),
                        OnRequest = c.Boolean(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ComplainID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .ForeignKey("dbo.ComplainTypes", t => t.ComplainTypeID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.LineStatus", t => t.LineStatusID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ClientDetailsID)
                .Index(t => t.EmployeeID)
                .Index(t => t.ResellerID)
                .Index(t => t.LineStatusID)
                .Index(t => t.ComplainTypeID);
            
            CreateTable(
                "dbo.ComplainTypes",
                c => new
                    {
                        ComplainTypeID = c.Int(nullable: false, identity: true),
                        ComplainTypeName = c.String(),
                        ShowMessageBox = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ComplainTypeID);
            
            CreateTable(
                "dbo.DirectProductSectionChangeFromWorkingToOthers",
                c => new
                    {
                        DirectProductSectionChangeFromWorkingToOthersID = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        TakenEmployee = c.String(),
                        StockDetailsID = c.Int(nullable: false),
                        FromSection = c.Int(nullable: false),
                        ToSection = c.Int(nullable: false),
                        WhoChanged = c.String(),
                        ChangeDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DirectProductSectionChangeFromWorkingToOthersID)
                .ForeignKey("dbo.StockDetails", t => t.StockDetailsID)
                .Index(t => t.StockDetailsID);
            
            CreateTable(
                "dbo.StockDetails",
                c => new
                    {
                        StockDetailsID = c.Int(nullable: false, identity: true),
                        StockID = c.Int(nullable: false),
                        BrandID = c.Int(),
                        SectionID = c.Int(nullable: false),
                        SupplierID = c.Int(),
                        SupplierInvoice = c.String(),
                        Serial = c.String(),
                        BarCode = c.String(),
                        ProductStatusID = c.Int(nullable: false),
                        WarrentyProduct = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StockDetailsID)
                .ForeignKey("dbo.Brands", t => t.BrandID)
                .ForeignKey("dbo.ProductStatus", t => t.ProductStatusID)
                .ForeignKey("dbo.Sections", t => t.SectionID)
                .ForeignKey("dbo.Stocks", t => t.StockID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID)
                .Index(t => t.StockID)
                .Index(t => t.BrandID)
                .Index(t => t.SectionID)
                .Index(t => t.SupplierID)
                .Index(t => t.ProductStatusID);
            
            CreateTable(
                "dbo.ProductStatus",
                c => new
                    {
                        ProductStatusID = c.Int(nullable: false, identity: true),
                        ProductStatusName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProductStatusID);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        SectionID = c.Int(nullable: false, identity: true),
                        SectionName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SectionID);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Int(),
                        UsedStatus = c.Int(),
                    })
                .PrimaryKey(t => t.StockID)
                .ForeignKey("dbo.Items", t => t.ItemID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemID = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        ItemFor = c.Int(),
                        ItemCode = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ItemID);
            
            CreateTable(
                "dbo.Distributions",
                c => new
                    {
                        DistributionID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        StockDetailsID = c.Int(nullable: false),
                        PopID = c.Int(),
                        BoxID = c.Int(),
                        ClientDetailsID = c.Int(),
                        DistributionReasonID = c.Int(),
                        IndicatorStatus = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.DistributionID)
                .ForeignKey("dbo.Boxes", t => t.BoxID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .ForeignKey("dbo.DistributionReasons", t => t.DistributionReasonID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Pops", t => t.PopID)
                .ForeignKey("dbo.StockDetails", t => t.StockDetailsID)
                .Index(t => t.EmployeeID)
                .Index(t => t.StockDetailsID)
                .Index(t => t.PopID)
                .Index(t => t.BoxID)
                .Index(t => t.ClientDetailsID)
                .Index(t => t.DistributionReasonID);
            
            CreateTable(
                "dbo.DistributionReasons",
                c => new
                    {
                        DistributionReasonID = c.Int(nullable: false, identity: true),
                        DistributionReasonName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DistributionReasonID);
            
            CreateTable(
                "dbo.Pops",
                c => new
                    {
                        PopID = c.Int(nullable: false, identity: true),
                        PopName = c.String(),
                        PopLocation = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PopID);
            
            CreateTable(
                "dbo.EmployeeLeaveHistories",
                c => new
                    {
                        EmployeeLeaveHistoryID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        Reason = c.String(),
                        LeaveType = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        LeaveTypes_LeaveTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeLeaveHistoryID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.LeaveSallaryTypes", t => t.LeaveTypes_LeaveTypeId)
                .Index(t => t.EmployeeID)
                .Index(t => t.LeaveTypes_LeaveTypeId);
            
            CreateTable(
                "dbo.LeaveSallaryTypes",
                c => new
                    {
                        LeaveTypeId = c.Int(nullable: false, identity: true),
                        LeaveTypeName = c.String(),
                        Persent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TableStatusID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LeaveTypeId);
            
            CreateTable(
                "dbo.EmployeeTransactionLockUnlocks",
                c => new
                    {
                        EmployeeTransactionLockUnlockID = c.Int(nullable: false, identity: true),
                        TransactionID = c.Int(nullable: false),
                        PackageID = c.Int(),
                        Amount = c.Double(nullable: false),
                        AmountForReseller = c.Double(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        LockOrUnlockDate = c.DateTime(),
                        EmployeeID = c.Int(),
                        ResellerID = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeTransactionLockUnlockID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Packages", t => t.PackageID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .ForeignKey("dbo.Transactions", t => t.TransactionID)
                .Index(t => t.TransactionID)
                .Index(t => t.PackageID)
                .Index(t => t.EmployeeID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        ClientDetailsID = c.Int(nullable: false),
                        PaymentYear = c.Int(nullable: false),
                        PaymentMonth = c.Int(nullable: false),
                        PackageID = c.Int(),
                        PaymentTypeID = c.Int(nullable: false),
                        PaymentFrom = c.Int(),
                        PaymentAmount = c.Single(),
                        ResellerPaymentAmount = c.Single(),
                        PackagePriceForResellerByAdminDuringCreateOrUpdate = c.Single(),
                        PackagePriceForResellerUserByResellerDuringCreateOrUpdate = c.Single(),
                        PaidAmount = c.Single(),
                        DueAmount = c.Single(),
                        PaymentStatus = c.Int(nullable: false),
                        Discount = c.Single(),
                        WhoGenerateTheBill = c.Int(),
                        EmployeeID = c.Int(),
                        BillCollectBy = c.Int(),
                        RemarksNo = c.String(),
                        ResetNo = c.String(),
                        PaymentDate = c.DateTime(),
                        LineStatusID = c.Int(),
                        AmountCountDate = c.DateTime(),
                        IsNewClient = c.Int(nullable: false),
                        ChangePackageHowMuchTimes = c.Int(nullable: false),
                        ForWhichSignUpBills = c.Int(),
                        PaymentFromWhichPage = c.String(),
                        ResellerID = c.Int(),
                        PaymentGenerateUptoWhichDate = c.DateTime(),
                        TransactionForWhichCycle = c.String(),
                        PermanentDiscount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.LineStatus", t => t.LineStatusID)
                .ForeignKey("dbo.Packages", t => t.PackageID)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentTypeID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ClientDetailsID)
                .Index(t => t.PackageID)
                .Index(t => t.PaymentTypeID)
                .Index(t => t.EmployeeID)
                .Index(t => t.LineStatusID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        PaymentTypeID = c.Int(nullable: false, identity: true),
                        PaymentTypeName = c.String(),
                    })
                .PrimaryKey(t => t.PaymentTypeID);
            
            CreateTable(
                "dbo.EmployeeVsWorkSchedules",
                c => new
                    {
                        EmployeeVsWorkScheduleID = c.Int(nullable: false, identity: true),
                        DayID = c.Int(nullable: false),
                        StartHour = c.Int(nullable: false),
                        StartMinute = c.Int(nullable: false),
                        RunHour = c.Int(nullable: false),
                        RunMinute = c.Int(nullable: false),
                        BreakStartHour = c.Int(nullable: false),
                        BreakStartMinute = c.Int(nullable: false),
                        BreakEndHour = c.Int(nullable: false),
                        BreakEndMinute = c.Int(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EmployeeVsWorkScheduleID)
                .ForeignKey("dbo.Days", t => t.DayID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .Index(t => t.DayID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Details = c.String(),
                        PaidTo = c.String(),
                        Amount = c.Double(nullable: false),
                        EmployeeID = c.Int(),
                        ResellerID = c.Int(),
                        PaymentDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ExpenseID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.EmployeeID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.GivenPaymentTypes",
                c => new
                    {
                        GivenPaymentTypeID = c.Int(nullable: false, identity: true),
                        GivenPaymentTypeName = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.String(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.GivenPaymentTypeID);
            
            CreateTable(
                "dbo.ISPAccessLists",
                c => new
                    {
                        ISPAccessListID = c.Int(nullable: false, identity: true),
                        AccessName = c.String(),
                        AccessValue = c.Int(nullable: false),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        ShowingStatus = c.Int(nullable: false),
                        IsGranted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ISPAccessListID);
            
            CreateTable(
                "dbo.MacResellerVSUserPaymentDeductionDetails",
                c => new
                    {
                        MacResellerVSUserPaymentDeductionDetailsID = c.Int(nullable: false, identity: true),
                        ClientDetailsID = c.Int(nullable: false),
                        ResellerID = c.Int(nullable: false),
                        PaymentYear = c.Int(nullable: false),
                        PaymentMonth = c.Int(nullable: false),
                        PaymentAmount = c.Double(nullable: false),
                        PaymentTime = c.DateTime(nullable: false),
                        PaymentTimeResellerBalance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.MacResellerVSUserPaymentDeductionDetailsID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ClientDetailsID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.MeasurementUnits",
                c => new
                    {
                        MeasurementUnitID = c.Int(nullable: false, identity: true),
                        UnitName = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MeasurementUnitID);
            
            CreateTable(
                "dbo.Months",
                c => new
                    {
                        MonthID = c.Int(nullable: false, identity: true),
                        MonthName = c.String(),
                    })
                .PrimaryKey(t => t.MonthID);
            
            CreateTable(
                "dbo.OptionSettings",
                c => new
                    {
                        OptionSettingsID = c.Int(nullable: false, identity: true),
                        OptionSettingsName = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OptionSettingsID);
            
            CreateTable(
                "dbo.PaymentBies",
                c => new
                    {
                        PaymentByID = c.Int(nullable: false, identity: true),
                        PaymentByName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentByID);
            
            CreateTable(
                "dbo.PaymentHistories",
                c => new
                    {
                        PaymentHistoryID = c.Int(nullable: false, identity: true),
                        TransactionID = c.Int(),
                        ClientDetailsID = c.Int(nullable: false),
                        EmployeeID = c.Int(),
                        ResellerID = c.Int(),
                        CollectByID = c.Int(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        PaidAmount = c.Single(nullable: false),
                        ResetNo = c.String(),
                        Status = c.Int(nullable: false),
                        AdvancePaymentID = c.Int(),
                        PaymentByID = c.Int(),
                        NormalPayment = c.Int(),
                        DiscountPayment = c.Int(),
                    })
                .PrimaryKey(t => t.PaymentHistoryID)
                .ForeignKey("dbo.AdvancePayments", t => t.AdvancePaymentID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.PaymentBies", t => t.PaymentByID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .ForeignKey("dbo.Transactions", t => t.TransactionID)
                .Index(t => t.TransactionID)
                .Index(t => t.ClientDetailsID)
                .Index(t => t.EmployeeID)
                .Index(t => t.ResellerID)
                .Index(t => t.AdvancePaymentID)
                .Index(t => t.PaymentByID);
            
            CreateTable(
                "dbo.ProfilePercentageFields",
                c => new
                    {
                        ProfilePercentageFieldsID = c.Int(nullable: false, identity: true),
                        FieldsName = c.String(),
                        TableName = c.String(),
                        MappingField = c.String(),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        UpdateBy = c.Int(nullable: false),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProfilePercentageFieldsID);
            
            CreateTable(
                "dbo.Purchase",
                c => new
                    {
                        PurchaseID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        SupplierID = c.Int(nullable: false),
                        PublishStatus = c.Int(nullable: false),
                        InvoicePrefix = c.String(),
                        InvoiceID = c.String(),
                        IssuedAt = c.DateTime(nullable: false),
                        SupplierNoted = c.String(),
                        SubTotal = c.Double(nullable: false),
                        DiscountType = c.Int(nullable: false),
                        DiscountPercentOrFixedAmount = c.Double(nullable: false),
                        DiscountAmount = c.Double(nullable: false),
                        Discount = c.Double(nullable: false),
                        Tax = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        PurchasePayment = c.Double(nullable: false),
                        ResellerID = c.Int(),
                        PurchaseStatus = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PurchaseID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID)
                .Index(t => t.SupplierID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.PurchaseDeatils",
                c => new
                    {
                        PurchaseDeatilsID = c.Int(nullable: false, identity: true),
                        PurchaseID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                        Tax = c.Double(nullable: false),
                        HasWarrenty = c.Boolean(nullable: false),
                        WarrentyStart = c.DateTime(),
                        WarrentyEnd = c.DateTime(),
                        Serial = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PurchaseDeatilsID)
                .ForeignKey("dbo.Items", t => t.ItemID)
                .ForeignKey("dbo.Purchase", t => t.PurchaseID)
                .Index(t => t.PurchaseID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.Recoveries",
                c => new
                    {
                        RecoveryID = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        DistributionReasonID = c.Int(nullable: false),
                        DistributionID = c.Int(nullable: false),
                        StockDetailsID = c.Int(nullable: false),
                        PopID = c.Int(),
                        BoxID = c.Int(),
                        ClientDetailsID = c.Int(),
                        RecoveryDate = c.DateTime(nullable: false),
                        IndicatorStatus = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RecoveryID)
                .ForeignKey("dbo.Boxes", t => t.BoxID)
                .ForeignKey("dbo.ClientDetails", t => t.ClientDetailsID)
                .ForeignKey("dbo.Distributions", t => t.DistributionID)
                .ForeignKey("dbo.DistributionReasons", t => t.DistributionReasonID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .ForeignKey("dbo.Pops", t => t.PopID)
                .ForeignKey("dbo.StockDetails", t => t.StockDetailsID)
                .Index(t => t.EmployeeID)
                .Index(t => t.DistributionReasonID)
                .Index(t => t.DistributionID)
                .Index(t => t.StockDetailsID)
                .Index(t => t.PopID)
                .Index(t => t.BoxID)
                .Index(t => t.ClientDetailsID);
            
            CreateTable(
                "dbo.Remarks",
                c => new
                    {
                        RemarksID = c.Int(nullable: false, identity: true),
                        RemarksNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RemarksID);
            
            CreateTable(
                "dbo.ResellerBillingCycles",
                c => new
                    {
                        ResellerBillingCycleID = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.String(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ResellerBillingCycleID);
            
            CreateTable(
                "dbo.ResellerPaymentDetailsHistories",
                c => new
                    {
                        ResellerPaymentID = c.Int(nullable: false, identity: true),
                        ResellerID = c.Int(nullable: false),
                        ResellerPaymentGivenTypeID = c.Int(nullable: false),
                        ActionTypeID = c.Int(nullable: false),
                        LastAmount = c.Double(nullable: false),
                        PaymentAmount = c.Double(nullable: false),
                        DeleteTimeResellerAmount = c.Double(nullable: false),
                        PaymentYear = c.Double(nullable: false),
                        PaymentMonth = c.Double(nullable: false),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentCheckOrAnySerial = c.String(),
                        Status = c.Int(nullable: false),
                        CollectBy = c.Int(nullable: false),
                        ActiveBy = c.Int(nullable: false),
                        PaymentByID = c.Int(nullable: false),
                        PaymenReceivedDate = c.DateTime(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ResellerPaymentID)
                .ForeignKey("dbo.PaymentBies", t => t.PaymentByID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ResellerID)
                .Index(t => t.PaymentByID);
            
            CreateTable(
                "dbo.ResellerVSPackageHistories",
                c => new
                    {
                        ResellerVSPackageHistoryID = c.Int(nullable: false, identity: true),
                        ResellerID = c.Int(nullable: false),
                        ResellerName = c.Int(nullable: false),
                        ResellerPackageID = c.Int(nullable: false),
                        PackageName = c.String(),
                        PackagePrice = c.String(),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        DeleteBy = c.Int(nullable: false),
                        DeleteDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResellerVSPackageHistoryID)
                .ForeignKey("dbo.Resellers", t => t.ResellerID)
                .Index(t => t.ResellerID);
            
            CreateTable(
                "dbo.Serials",
                c => new
                    {
                        SerialID = c.Int(nullable: false, identity: true),
                        SerialNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SerialID);
            
            CreateTable(
                "dbo.SerialNoForAdvancePayments",
                c => new
                    {
                        SerialNoForAdvancePaymentID = c.Int(nullable: false, identity: true),
                        SerialNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SerialNoForAdvancePaymentID);
            
            CreateTable(
                "dbo.SMS",
                c => new
                    {
                        SMSID = c.Int(nullable: false, identity: true),
                        SMSTitle = c.String(),
                        SendMessageText = c.String(),
                        SMSCode = c.String(),
                        Sender = c.String(),
                        SMSStatus = c.Int(nullable: false),
                        SMSCounter = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        UpdateBy = c.Int(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SMSID);
            
            CreateTable(
                "dbo.SMSSenderIDPasses",
                c => new
                    {
                        SMSSenderIDPassID = c.Int(nullable: false, identity: true),
                        ID = c.String(),
                        Pass = c.String(),
                        Sender = c.String(),
                        CompanyName = c.String(),
                        Status = c.Int(nullable: false),
                        HelpLine = c.String(),
                        CreateBy = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SMSSenderIDPassID);
            
            CreateTable(
                "dbo.TimePeriodForSignals",
                c => new
                    {
                        TimePeriodForSignalID = c.Int(nullable: false, identity: true),
                        UpToHours = c.Double(nullable: false),
                        SignalSign = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        UpdateBy = c.String(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TimePeriodForSignalID);
            
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        TokenID = c.Int(nullable: false, identity: true),
                        TokenNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TokenID);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        VendorID = c.Int(nullable: false, identity: true),
                        VendorName = c.String(),
                        VendorAddress = c.String(),
                        CompanyName = c.String(),
                        VendorLogoName = c.String(),
                        VendorImageOriginalName = c.Binary(),
                        VendorImagePath = c.String(),
                        VendorContactPerson = c.String(),
                        VendorEmail = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                        VendorTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VendorID)
                .ForeignKey("dbo.VendorTypes", t => t.VendorTypeID)
                .Index(t => t.VendorTypeID);
            
            CreateTable(
                "dbo.VendorTypes",
                c => new
                    {
                        VendorTypeID = c.Int(nullable: false, identity: true),
                        VendorTypeName = c.String(),
                        Status = c.Int(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateBy = c.Int(),
                        UpdateDate = c.DateTime(),
                        DeleteBy = c.Int(),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VendorTypeID);
            
            CreateTable(
                "dbo.Years",
                c => new
                    {
                        YearID = c.Int(nullable: false, identity: true),
                        YearName = c.String(),
                    })
                .PrimaryKey(t => t.YearID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "VendorTypeID", "dbo.VendorTypes");
            DropForeignKey("dbo.ResellerVSPackageHistories", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.ResellerPaymentDetailsHistories", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.ResellerPaymentDetailsHistories", "PaymentByID", "dbo.PaymentBies");
            DropForeignKey("dbo.Recoveries", "StockDetailsID", "dbo.StockDetails");
            DropForeignKey("dbo.Recoveries", "PopID", "dbo.Pops");
            DropForeignKey("dbo.Recoveries", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Recoveries", "DistributionReasonID", "dbo.DistributionReasons");
            DropForeignKey("dbo.Recoveries", "DistributionID", "dbo.Distributions");
            DropForeignKey("dbo.Recoveries", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.Recoveries", "BoxID", "dbo.Boxes");
            DropForeignKey("dbo.PurchaseDeatils", "PurchaseID", "dbo.Purchase");
            DropForeignKey("dbo.PurchaseDeatils", "ItemID", "dbo.Items");
            DropForeignKey("dbo.Purchase", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.Purchase", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.PaymentHistories", "TransactionID", "dbo.Transactions");
            DropForeignKey("dbo.PaymentHistories", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.PaymentHistories", "PaymentByID", "dbo.PaymentBies");
            DropForeignKey("dbo.PaymentHistories", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.PaymentHistories", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.PaymentHistories", "AdvancePaymentID", "dbo.AdvancePayments");
            DropForeignKey("dbo.MacResellerVSUserPaymentDeductionDetails", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.MacResellerVSUserPaymentDeductionDetails", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.Expenses", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.Expenses", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeVsWorkSchedules", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeVsWorkSchedules", "DayID", "dbo.Days");
            DropForeignKey("dbo.EmployeeTransactionLockUnlocks", "TransactionID", "dbo.Transactions");
            DropForeignKey("dbo.Transactions", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.Transactions", "PaymentTypeID", "dbo.PaymentTypes");
            DropForeignKey("dbo.Transactions", "PackageID", "dbo.Packages");
            DropForeignKey("dbo.Transactions", "LineStatusID", "dbo.LineStatus");
            DropForeignKey("dbo.Transactions", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Transactions", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.EmployeeTransactionLockUnlocks", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.EmployeeTransactionLockUnlocks", "PackageID", "dbo.Packages");
            DropForeignKey("dbo.EmployeeTransactionLockUnlocks", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeLeaveHistories", "LeaveTypes_LeaveTypeId", "dbo.LeaveSallaryTypes");
            DropForeignKey("dbo.EmployeeLeaveHistories", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Distributions", "StockDetailsID", "dbo.StockDetails");
            DropForeignKey("dbo.Distributions", "PopID", "dbo.Pops");
            DropForeignKey("dbo.Distributions", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Distributions", "DistributionReasonID", "dbo.DistributionReasons");
            DropForeignKey("dbo.Distributions", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.Distributions", "BoxID", "dbo.Boxes");
            DropForeignKey("dbo.DirectProductSectionChangeFromWorkingToOthers", "StockDetailsID", "dbo.StockDetails");
            DropForeignKey("dbo.StockDetails", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.StockDetails", "StockID", "dbo.Stocks");
            DropForeignKey("dbo.Stocks", "ItemID", "dbo.Items");
            DropForeignKey("dbo.StockDetails", "SectionID", "dbo.Sections");
            DropForeignKey("dbo.StockDetails", "ProductStatusID", "dbo.ProductStatus");
            DropForeignKey("dbo.StockDetails", "BrandID", "dbo.Brands");
            DropForeignKey("dbo.Complains", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.Complains", "LineStatusID", "dbo.LineStatus");
            DropForeignKey("dbo.Complains", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Complains", "ComplainTypeID", "dbo.ComplainTypes");
            DropForeignKey("dbo.Complains", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.ClientLineStatus", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.ClientLineStatus", "PackageID", "dbo.Packages");
            DropForeignKey("dbo.ClientLineStatus", "MikrotikID", "dbo.Mikrotiks");
            DropForeignKey("dbo.ClientLineStatus", "LineStatusID", "dbo.LineStatus");
            DropForeignKey("dbo.ClientLineStatus", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.ClientLineStatus", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.ClientDueBills", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.ClientBannedStatus", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.ClientBannedStatus", "BannedStatusID", "dbo.BannedStatus");
            DropForeignKey("dbo.CableDistributions", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.CableDistributions", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.CableDistributions", "CableStockID", "dbo.CableStocks");
            DropForeignKey("dbo.CableStocks", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.CableStocks", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.CableStocks", "CableUnitID", "dbo.CableUnits");
            DropForeignKey("dbo.ClientDetails", "CableUnit_CableUnitID", "dbo.CableUnits");
            DropForeignKey("dbo.CableStocks", "CableTypeID", "dbo.CableTypes");
            DropForeignKey("dbo.CableStocks", "BrandID", "dbo.Brands");
            DropForeignKey("dbo.Boxes", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.AtendaceInOuts", "AttendanceTypeID", "dbo.AttendanceTypes");
            DropForeignKey("dbo.Assets", "AssetTypeID", "dbo.AssetTypes");
            DropForeignKey("dbo.AdvancePayments", "ClientDetailsID", "dbo.ClientDetails");
            DropForeignKey("dbo.Zones", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.ClientDetails", "ZoneID", "dbo.Zones");
            DropForeignKey("dbo.ClientDetails", "UserRightPermissionID", "dbo.UserRightPermissions");
            DropForeignKey("dbo.ClientDetails", "SecurityQuestionID", "dbo.SecurityQuestions");
            DropForeignKey("dbo.ClientDetails", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.ClientDetails", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.ClientDetails", "PackageID", "dbo.Packages");
            DropForeignKey("dbo.Packages", "MikrotikID", "dbo.Mikrotiks");
            DropForeignKey("dbo.Packages", "IPPoolID", "dbo.IPPools");
            DropForeignKey("dbo.ClientDetails", "MikrotikID", "dbo.Mikrotiks");
            DropForeignKey("dbo.ClientDetails", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Employees", "UserRightPermissionID", "dbo.UserRightPermissions");
            DropForeignKey("dbo.Employees", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Employees", "ResellerID", "dbo.Resellers");
            DropForeignKey("dbo.Resellers", "UserRightPermissionID", "dbo.UserRightPermissions");
            DropForeignKey("dbo.Resellers", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Employees", "DutyShiftID", "dbo.DutyShifts");
            DropForeignKey("dbo.Employees", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Employees", "DayID", "dbo.Days");
            DropForeignKey("dbo.ClientDetails", "ConnectionTypeID", "dbo.ConnectionTypes");
            DropForeignKey("dbo.ClientDetails", "CableTypeID", "dbo.CableTypes");
            DropForeignKey("dbo.ActionLists", "FormID", "dbo.Forms");
            DropForeignKey("dbo.Forms", "ControllerNameID", "dbo.ControllerNames");
            DropIndex("dbo.Vendors", new[] { "VendorTypeID" });
            DropIndex("dbo.ResellerVSPackageHistories", new[] { "ResellerID" });
            DropIndex("dbo.ResellerPaymentDetailsHistories", new[] { "PaymentByID" });
            DropIndex("dbo.ResellerPaymentDetailsHistories", new[] { "ResellerID" });
            DropIndex("dbo.Recoveries", new[] { "ClientDetailsID" });
            DropIndex("dbo.Recoveries", new[] { "BoxID" });
            DropIndex("dbo.Recoveries", new[] { "PopID" });
            DropIndex("dbo.Recoveries", new[] { "StockDetailsID" });
            DropIndex("dbo.Recoveries", new[] { "DistributionID" });
            DropIndex("dbo.Recoveries", new[] { "DistributionReasonID" });
            DropIndex("dbo.Recoveries", new[] { "EmployeeID" });
            DropIndex("dbo.PurchaseDeatils", new[] { "ItemID" });
            DropIndex("dbo.PurchaseDeatils", new[] { "PurchaseID" });
            DropIndex("dbo.Purchase", new[] { "ResellerID" });
            DropIndex("dbo.Purchase", new[] { "SupplierID" });
            DropIndex("dbo.PaymentHistories", new[] { "PaymentByID" });
            DropIndex("dbo.PaymentHistories", new[] { "AdvancePaymentID" });
            DropIndex("dbo.PaymentHistories", new[] { "ResellerID" });
            DropIndex("dbo.PaymentHistories", new[] { "EmployeeID" });
            DropIndex("dbo.PaymentHistories", new[] { "ClientDetailsID" });
            DropIndex("dbo.PaymentHistories", new[] { "TransactionID" });
            DropIndex("dbo.MacResellerVSUserPaymentDeductionDetails", new[] { "ResellerID" });
            DropIndex("dbo.MacResellerVSUserPaymentDeductionDetails", new[] { "ClientDetailsID" });
            DropIndex("dbo.Expenses", new[] { "ResellerID" });
            DropIndex("dbo.Expenses", new[] { "EmployeeID" });
            DropIndex("dbo.EmployeeVsWorkSchedules", new[] { "EmployeeID" });
            DropIndex("dbo.EmployeeVsWorkSchedules", new[] { "DayID" });
            DropIndex("dbo.Transactions", new[] { "ResellerID" });
            DropIndex("dbo.Transactions", new[] { "LineStatusID" });
            DropIndex("dbo.Transactions", new[] { "EmployeeID" });
            DropIndex("dbo.Transactions", new[] { "PaymentTypeID" });
            DropIndex("dbo.Transactions", new[] { "PackageID" });
            DropIndex("dbo.Transactions", new[] { "ClientDetailsID" });
            DropIndex("dbo.EmployeeTransactionLockUnlocks", new[] { "ResellerID" });
            DropIndex("dbo.EmployeeTransactionLockUnlocks", new[] { "EmployeeID" });
            DropIndex("dbo.EmployeeTransactionLockUnlocks", new[] { "PackageID" });
            DropIndex("dbo.EmployeeTransactionLockUnlocks", new[] { "TransactionID" });
            DropIndex("dbo.EmployeeLeaveHistories", new[] { "LeaveTypes_LeaveTypeId" });
            DropIndex("dbo.EmployeeLeaveHistories", new[] { "EmployeeID" });
            DropIndex("dbo.Distributions", new[] { "DistributionReasonID" });
            DropIndex("dbo.Distributions", new[] { "ClientDetailsID" });
            DropIndex("dbo.Distributions", new[] { "BoxID" });
            DropIndex("dbo.Distributions", new[] { "PopID" });
            DropIndex("dbo.Distributions", new[] { "StockDetailsID" });
            DropIndex("dbo.Distributions", new[] { "EmployeeID" });
            DropIndex("dbo.Stocks", new[] { "ItemID" });
            DropIndex("dbo.StockDetails", new[] { "ProductStatusID" });
            DropIndex("dbo.StockDetails", new[] { "SupplierID" });
            DropIndex("dbo.StockDetails", new[] { "SectionID" });
            DropIndex("dbo.StockDetails", new[] { "BrandID" });
            DropIndex("dbo.StockDetails", new[] { "StockID" });
            DropIndex("dbo.DirectProductSectionChangeFromWorkingToOthers", new[] { "StockDetailsID" });
            DropIndex("dbo.Complains", new[] { "ComplainTypeID" });
            DropIndex("dbo.Complains", new[] { "LineStatusID" });
            DropIndex("dbo.Complains", new[] { "ResellerID" });
            DropIndex("dbo.Complains", new[] { "EmployeeID" });
            DropIndex("dbo.Complains", new[] { "ClientDetailsID" });
            DropIndex("dbo.ClientLineStatus", new[] { "MikrotikID" });
            DropIndex("dbo.ClientLineStatus", new[] { "ResellerID" });
            DropIndex("dbo.ClientLineStatus", new[] { "EmployeeID" });
            DropIndex("dbo.ClientLineStatus", new[] { "LineStatusID" });
            DropIndex("dbo.ClientLineStatus", new[] { "PackageID" });
            DropIndex("dbo.ClientLineStatus", new[] { "ClientDetailsID" });
            DropIndex("dbo.ClientDueBills", new[] { "ClientDetailsID" });
            DropIndex("dbo.ClientBannedStatus", new[] { "BannedStatusID" });
            DropIndex("dbo.ClientBannedStatus", new[] { "ClientDetailsID" });
            DropIndex("dbo.CableStocks", new[] { "EmployeeID" });
            DropIndex("dbo.CableStocks", new[] { "CableUnitID" });
            DropIndex("dbo.CableStocks", new[] { "SupplierID" });
            DropIndex("dbo.CableStocks", new[] { "BrandID" });
            DropIndex("dbo.CableStocks", new[] { "CableTypeID" });
            DropIndex("dbo.CableDistributions", new[] { "CableStockID" });
            DropIndex("dbo.CableDistributions", new[] { "EmployeeID" });
            DropIndex("dbo.CableDistributions", new[] { "ClientDetailsID" });
            DropIndex("dbo.Boxes", new[] { "ResellerID" });
            DropIndex("dbo.AtendaceInOuts", new[] { "AttendanceTypeID" });
            DropIndex("dbo.Assets", new[] { "AssetTypeID" });
            DropIndex("dbo.Zones", new[] { "ResellerID" });
            DropIndex("dbo.Packages", new[] { "MikrotikID" });
            DropIndex("dbo.Packages", new[] { "IPPoolID" });
            DropIndex("dbo.Resellers", new[] { "UserRightPermissionID" });
            DropIndex("dbo.Resellers", new[] { "RoleID" });
            DropIndex("dbo.Employees", new[] { "ResellerID" });
            DropIndex("dbo.Employees", new[] { "DayID" });
            DropIndex("dbo.Employees", new[] { "DutyShiftID" });
            DropIndex("dbo.Employees", new[] { "UserRightPermissionID" });
            DropIndex("dbo.Employees", new[] { "RoleID" });
            DropIndex("dbo.Employees", new[] { "DepartmentID" });
            DropIndex("dbo.ClientDetails", new[] { "CableUnit_CableUnitID" });
            DropIndex("dbo.ClientDetails", new[] { "ResellerID" });
            DropIndex("dbo.ClientDetails", new[] { "MikrotikID" });
            DropIndex("dbo.ClientDetails", new[] { "UserRightPermissionID" });
            DropIndex("dbo.ClientDetails", new[] { "RoleID" });
            DropIndex("dbo.ClientDetails", new[] { "EmployeeID" });
            DropIndex("dbo.ClientDetails", new[] { "SecurityQuestionID" });
            DropIndex("dbo.ClientDetails", new[] { "PackageID" });
            DropIndex("dbo.ClientDetails", new[] { "CableTypeID" });
            DropIndex("dbo.ClientDetails", new[] { "ConnectionTypeID" });
            DropIndex("dbo.ClientDetails", new[] { "ZoneID" });
            DropIndex("dbo.AdvancePayments", new[] { "ClientDetailsID" });
            DropIndex("dbo.Forms", new[] { "ControllerNameID" });
            DropIndex("dbo.ActionLists", new[] { "FormID" });
            DropTable("dbo.Years");
            DropTable("dbo.VendorTypes");
            DropTable("dbo.Vendors");
            DropTable("dbo.Tokens");
            DropTable("dbo.TimePeriodForSignals");
            DropTable("dbo.SMSSenderIDPasses");
            DropTable("dbo.SMS");
            DropTable("dbo.SerialNoForAdvancePayments");
            DropTable("dbo.Serials");
            DropTable("dbo.ResellerVSPackageHistories");
            DropTable("dbo.ResellerPaymentDetailsHistories");
            DropTable("dbo.ResellerBillingCycles");
            DropTable("dbo.Remarks");
            DropTable("dbo.Recoveries");
            DropTable("dbo.PurchaseDeatils");
            DropTable("dbo.Purchase");
            DropTable("dbo.ProfilePercentageFields");
            DropTable("dbo.PaymentHistories");
            DropTable("dbo.PaymentBies");
            DropTable("dbo.OptionSettings");
            DropTable("dbo.Months");
            DropTable("dbo.MeasurementUnits");
            DropTable("dbo.MacResellerVSUserPaymentDeductionDetails");
            DropTable("dbo.ISPAccessLists");
            DropTable("dbo.GivenPaymentTypes");
            DropTable("dbo.Expenses");
            DropTable("dbo.EmployeeVsWorkSchedules");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Transactions");
            DropTable("dbo.EmployeeTransactionLockUnlocks");
            DropTable("dbo.LeaveSallaryTypes");
            DropTable("dbo.EmployeeLeaveHistories");
            DropTable("dbo.Pops");
            DropTable("dbo.DistributionReasons");
            DropTable("dbo.Distributions");
            DropTable("dbo.Items");
            DropTable("dbo.Stocks");
            DropTable("dbo.Sections");
            DropTable("dbo.ProductStatus");
            DropTable("dbo.StockDetails");
            DropTable("dbo.DirectProductSectionChangeFromWorkingToOthers");
            DropTable("dbo.ComplainTypes");
            DropTable("dbo.Complains");
            DropTable("dbo.LineStatus");
            DropTable("dbo.ClientLineStatus");
            DropTable("dbo.ClientDueBills");
            DropTable("dbo.ClientBannedStatus");
            DropTable("dbo.Suppliers");
            DropTable("dbo.CableUnits");
            DropTable("dbo.CableStocks");
            DropTable("dbo.CableDistributions");
            DropTable("dbo.Brands");
            DropTable("dbo.Boxes");
            DropTable("dbo.BillGenerateHistories");
            DropTable("dbo.BannedStatus");
            DropTable("dbo.BandwithResellerGivenItems");
            DropTable("dbo.AttendanceTypes");
            DropTable("dbo.AtendaceInOuts");
            DropTable("dbo.AssetTypes");
            DropTable("dbo.Assets");
            DropTable("dbo.Zones");
            DropTable("dbo.SecurityQuestions");
            DropTable("dbo.IPPools");
            DropTable("dbo.Packages");
            DropTable("dbo.Mikrotiks");
            DropTable("dbo.UserRightPermissions");
            DropTable("dbo.Roles");
            DropTable("dbo.Resellers");
            DropTable("dbo.DutyShifts");
            DropTable("dbo.Departments");
            DropTable("dbo.Days");
            DropTable("dbo.Employees");
            DropTable("dbo.ConnectionTypes");
            DropTable("dbo.CableTypes");
            DropTable("dbo.ClientDetails");
            DropTable("dbo.AdvancePayments");
            DropTable("dbo.ControllerNames");
            DropTable("dbo.Forms");
            DropTable("dbo.ActionLists");
        }
    }
}
