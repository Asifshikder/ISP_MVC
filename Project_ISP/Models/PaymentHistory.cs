using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    //[Table("ph")]
    public class PaymentHistory
    {
        [Key]
        //[Column("phi")]
        public int PaymentHistoryID { get; set; }
        //[Column("ti")]
        public int? TransactionID { get; set; }
        public Transaction Transaction { get; set; }
        //[Column("cdi")]
        public int ClientDetailsID { get; set; }
        public ClientDetails ClientDetails { get; set; }
        //[Column("paidbyemp")]
        public int? EmployeeID { get; set; }//this the employe who change the status
        public virtual Employee Employee { get; set; }
        ////[Column("paidbyres")]
        public int? ResellerID { get; set; }//this the emreseller ploye who change the status
        public virtual Reseller Reseller { get; set; }
        //[Column("Collectby")]
        public int CollectByID { get; set; }

        //[Column("pd")]
        public DateTime PaymentDate { get; set; }
        //[Column("pa")]
        public float PaidAmount { get; set; }
        //[Column("rstNo")]
        public string ResetNo { get; set; }
        public int Status { get; set; }
        
        public int? AdvancePaymentID { get; set; }
        public AdvancePayment advancePayment { get; set; }
        public int? PaymentByID { get; set; }
        public PaymentBy PaymentBy { get; set; }

        public int? NormalPayment { get; set; }
        public int? DiscountPayment { get; set; } 
        public string PaymentFromWhichPage { get; set; }
        public int BillAcceptBy { get; set; }
        public bool AcceptStatus { get; set; }
    }
}