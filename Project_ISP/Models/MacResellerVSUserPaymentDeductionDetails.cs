using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class MacResellerVSUserPaymentDeductionDetails
    {
        [Key]
        public int MacResellerVSUserPaymentDeductionDetailsID { get; set; }
        public int ClientDetailsID { get; set; }
        public ClientDetails ClientDetails { get; set; }
        public int ResellerID { get; set; }
        public Reseller Reseller { get; set; }
        public int PaymentYear { get; set; }
        public int PaymentMonth { get; set; }
        public double PaymentAmount { get; set; }
        public DateTime PaymentTime { get; set; }
        public double PaymentTimeResellerBalance { get; set; }

    }
}