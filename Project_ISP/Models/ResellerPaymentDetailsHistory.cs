using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    //[Table("RP")]
    public class ResellerPaymentDetailsHistory
    {
        [Key]
        public int ResellerPaymentID { get; set; }
        public int ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }
        public int ResellerPaymentGivenTypeID { get; set; } //cash or check or some other.
        public int ActionTypeID { get; set; } //cash  purchase or cash purchase return
        public double LastAmount { get; set; }
        public double PaymentAmount { get; set; }
        public double DeleteTimeResellerAmount { get; set; }
        public double PaymentYear { get; set; }
        public double PaymentMonth { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentCheckOrAnySerial { get; set; }
        public int Status { get; set; }
        public int CollectBy { get; set; }
        public int ActiveBy { get; set; } 
        public int PaymentByID { get; set; }
        public PaymentBy PaymentBy { get; set; }

        public DateTime? PaymenReceivedDate { get; set; }

        //[Column("AG")]
        public int CreatedBy { get; set; }
        //[Column("AH")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AJ")]
        public int? UpdateBy { get; set; }
        //[Column("AK")]
        public DateTime? UpdateDate { get; set; }
        //[Column("rb")]
        public int? DeleteBy { get; set; }
        //[Column("rd")]
        public DateTime? DeleteDate { get; set; }
    }
}