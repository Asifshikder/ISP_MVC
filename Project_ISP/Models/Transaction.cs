using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    //[Table("40")]
    public class Transaction
    {
        [Key]
        //[Column("a1")]
        public int TransactionID { get; set; }
        //[Column("a2")]
        public int ClientDetailsID { get; set; }
        public virtual ClientDetails ClientDetails { get; set; }
        //[Column("a3")]
        public int PaymentYear { get; set; }
        //[Column("a4")]
        public int PaymentMonth { get; set; }
        //[Column("a5")]
        public int? PackageID { get; set; }
        public virtual Package Package { get; set; }
        //public int? ResellerPackageID { get; set; }
        //public virtual ResellerPackage ResellerPackage { get; set; }

        //[Column("a6")]
        public int PaymentTypeID { get; set; }
        public PaymentType PaymentType { get; set; }// what type of payment ? connection fee / Mohtnly fee
        //[Column("a7")]
        public int? PaymentFrom { get; set; }//advance or direct
        //[Column("a8")]
        public float? PaymentAmount { get; set; }//this Payment amount is for reseller user not for reseller payment amount
        public float? ResellerPaymentAmount { get; set; }//this Payment amount is for reseller payment amount not for reseller user
        public float? PackagePriceForResellerByAdminDuringCreateOrUpdate { get; set; }
        public float? PackagePriceForResellerUserByResellerDuringCreateOrUpdate { get; set; }
        //[Column("a24")]
        public float? PaidAmount { get; set; }//this paid amount is for reseller user not for reseller amount
        //[Column("a9")]
        public float? DueAmount { get; set; }//this Due amount is for reseller user not for reseller due
        //[Column("a10")]
        public int PaymentStatus { get; set; }// done or pending
        //[Column("a11")]
        public float? Discount { get; set; }
        //[Column("a12")]
        public int? WhoGenerateTheBill { get; set; }
        //[Column("a13")]
        public int? EmployeeID { get; set; }//ho paid the bail
        public virtual Employee Employee { get; set; }

        //[Column("a14")]
        public int? BillCollectBy { get; set; }
        //[Column("a15")]
        public string RemarksNo { get; set; }
        //[Column("a16")]
        public string ResetNo { get; set; }
        //[Column("a17")]
        public DateTime? PaymentDate { get; set; }
        //[Column("a18")]
        public int? LineStatusID { get; set; }// this is the id for latest line status change which will effet every hit except new connection mean paymenttype = new connection

        public virtual LineStatus LineStatus { get; set; }
        //[Column("a19")]
        public DateTime? AmountCountDate { get; set; } //this is the date from where amount will calculate and will store in emptsLockUnlock tbl  when status will be lock
        //[Column("a20")]
        public int IsNewClient { get; set; }
        //[Column("a21")]
        public int ChangePackageHowMuchTimes { get; set; }
        //[Column("a22")]
        public int? ForWhichSignUpBills { get; set; } //this field is created for identify which bill is generating after sign upbill.

        //[Column("a23")]
        public string PaymentFromWhichPage { get; set; }


        //[Column("ResellerID")]
        public int? ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }

        //public DateTime PaymentGenerateDate { get; set; } //this the bill generate time
        //public DateTime NextGenerateDate { get; set; }    //this is the next bill generate time]

        //this is the date for upto which date we generate our payment. specially for cycle client
        public DateTime? PaymentGenerateUptoWhichDate { get; set; }
        public string TransactionForWhichCycle { get; set; }
        public double PermanentDiscount { get; set; }
        [NotMapped]
        public string AnotherMobileNo { get; set; }
        [NotMapped]
        public int PaymentBy { get; set; }
    }
}