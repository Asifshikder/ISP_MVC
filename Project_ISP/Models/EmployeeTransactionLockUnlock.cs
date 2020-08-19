using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("7")]
    public class EmployeeTransactionLockUnlock
    {
        [Key]
        //[Column("AER")]
        public int EmployeeTransactionLockUnlockID { get; set; }
        //[Column("AW")]
        public int TransactionID { get; set; }
        public Transaction Transaction { get; set; }
        //[Column("AQ")]
        public int? PackageID { get; set; }
        public virtual Package Package { get; set; }
        //[Column("AF")]
        public double Amount { get; set; } //those days bill  user for resller or admin
        public double AmountForReseller { get; set; } //those days bill for reseller when we are adding amount for user for reseller

        //public int PaymentYear { get; set; }
        //public int PaymentMonth { get; set; }
        //[Column("AD")]
        public DateTime FromDate { get; set; } //from which date bill will be start
        //[Column("AS")]
        public DateTime ToDate { get; set; }   //where date bill will be finish
        //[Column("AV")]
        public DateTime? LockOrUnlockDate { get; set; } //when status is lock
        //[Column("A")]
        public int? EmployeeID { get; set; }//this the employe who change the status
        public virtual Employee Employee { get; set; }

        public int? ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }
    }
}