using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Data.Entity.Core.Objects;
using Project_ISP.Models;

namespace ISP_ManagementSystemModel.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ISPContext : DbContext
    {
        public ISPContext()
            : base("ISPConnectionString")
        {

        }


        public DbSet<ProfilePercentageFields> ProfilePercentageFields { get; set; }
        public DbSet<Complain> Complain { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Expense> Expenses { set; get; }
        public DbSet<AdvancePayment> AdvancePayment { get; set; }
        public DbSet<BannedStatus> BannedStatus { get; set; }
        public DbSet<CableType> CableType { get; set; }
        public DbSet<ClientBannedStatus> ClientBannedStatus { get; set; }
        public DbSet<ClientDetails> ClientDetails { get; set; }
        public DbSet<ClientLineStatus> ClientLineStatus { get; set; }
        public DbSet<ConnectionType> ConnectionType { get; set; }
        public DbSet<LineStatus> LineStatus { get; set; }
        public DbSet<Month> Month { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<PaymentType> PaymentStatus { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestion { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Year> Year { get; set; }
        public DbSet<Zone> Zone { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Employee> Employee { get; set; }
        //public DbSet<PaymentFrom> PaymentFrom { get; set; }
        public DbSet<Remarks> Remarks { get; set; }
        public DbSet<Serial> Serial { get; set; }
        public DbSet<Role> Role { get; set; }

        public DbSet<ClientDueBills> ClientDueBills { get; set; }


        public DbSet<Brand> Brand { get; set; }
        public DbSet<DistributionReason> DistributionReason { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<ProductStatus> ProductStatus { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Pop> Pop { get; set; }
        public DbSet<Box> Box { get; set; }
        public DbSet<Supplier> Supplier { get; set; }


        public DbSet<Stock> Stock { get; set; }
        public DbSet<StockDetails> StockDetails { get; set; }
        public DbSet<Distribution> Distribution { get; set; }
        public DbSet<Recovery> Recovery { get; set; }
        public DbSet<CableUnit> CableUnit { get; set; }
        public DbSet<CableStock> CableStock { get; set; }


        public DbSet<CableDistribution> CableDistribution { get; set; }

        public DbSet<ControllerName> ControllerName { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<ActionList> Action { get; set; }
        public DbSet<UserRightPermission> UserRightPermission { get; set; }
        public DbSet<ISPAccessList> ISPAccessList { get; set; }

        public DbSet<EmployeeTransactionLockUnlock> EmployeeTransactionLockUnlock { get; set; }
        public DbSet<SerialNoForAdvancePayment> SerialNoForAdvancePayment { get; set; }
        public DbSet<SMS> SMS { get; set; }
        public DbSet<SMSSenderIDPass> SMSSenderIDPass { get; set; }


        public DbSet<Mikrotik> Mikrotik { get; set; }
        public DbSet<IPPool> IPPool { get; set; }
        public DbSet<OptionSettings> OptionSettings { get; set; }

        public DbSet<DirectProductSectionChangeFromWorkingToOthers> DirectProductSectionChangeFromWorkingToOthers { get; set; }

        public DbSet<Reseller> Reseller { get; set; }
        public DbSet<ComplainType> ComplainType { get; set; }


        public DbSet<Asset> Asset { get; set; }
        public DbSet<AssetType> AssetType { get; set; }

        public DbSet<TimePeriodForSignal> TimePeriodForSignal { get; set; }
        public DbSet<PaymentHistory> PaymentHistory { get; set; }
        public DbSet<BillGenerateHistory> BIllGenerateHistory { get; set; }
        public DbSet<PaymentBy> PaymentBy { get; set; }

        //public DbSet<ResellerPackage> ResellerPackage { get; set; }
        public DbSet<ResellerBillingCycle> ResellerBillingCycle { get; set; }

        public DbSet<ResellerVSPackageHistory> ResellerVSPackageHistory { get; set; }
        public DbSet<BandwithResellerGivenItem> BandwithResellerGivenItem { get; set; }
        public DbSet<ResellerPaymentDetailsHistory> ResellerPaymentDetailsHistory { get; set; }
        public DbSet<GivenPaymentType> GivenPaymentType { get; set; }
        public DbSet<MacResellerVSUserPaymentDeductionDetails> MacResellerVSUserPaymentDeductionDetails { get; set; }
        public DbSet<AttendanceType> AttendanceTypes { get; set; }
        public DbSet<AtendaceInOut> AtendaceInOuts { get; set; }
        public DbSet<LeaveSallaryType> LeaveSallaryTypes { get; set; }
        public DbSet<EmployeeLeaveHistory> EmployeeLeaveHistories { get; set; }
        public DbSet<EmployeeVsWorkSchedule> EmployeeVsWorkSchedules { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<DutyShift> DutyShifts { get; set; }
        public DbSet<MeasurementUnits> MeasurementUnits { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }
        public DbSet<Vendor> Vendor { get; set; }

        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<PurchaseDeatils> PurchaseDeatils { get; set; }
        public DbSet<PurchasePaymentHistory> PurchasePaymentHistory { get; set; }


        public DbSet<AccountOwner> AccountOwner { get; set; }
        public DbSet<AccountList> AccountList { get; set; }

        public DbSet<Head> Head { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyVSPayer> CompanyVSPayer { get; set; }

        public DbSet<Deposit> Deposit { get; set; }
        public DbSet<AccountingHistory> AccountingHistory { get; set; }

        public DbSet<AccountListVsAmountTransfer> AccountListVsAmountTransfer { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // modelBuilder.Entity<Foo>().HasOptional(f => f.Boo).WithRequired(s => s.Foo);
            //    Database.SetInitializer<SMSContext>(null);
            //    base.OnModelCreating(modelBuilder);
        }
    }
}