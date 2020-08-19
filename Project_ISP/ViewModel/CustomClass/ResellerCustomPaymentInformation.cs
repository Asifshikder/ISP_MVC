using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel.CustomClass
{
    public class ResellerCustomPaymentInformation
    {
        public int ResellerPaymentID { get; set; }
        public int ResellerID { get; set; }
        public string RID { get; set; }
        public string ResellerName { get; set; }
        public string ResellerLoginName { get; set; }
        public string ResellerBusinessName { get; set; }
        public string ResellerAddress { get; set; }
        public string ResellerPhone { get; set; }
        public string ResellerStatus { get; set; }
        public string PaymentTypeID { get; set; } //cash or check or some other.
        public string ActionTypeID { get; set; } //cash  purchase or cash purchase return
        public double PaymentAmount { get; set; }
        public double PaymentYear { get; set; }
        public double PaymentMonth { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentCheckOrAnySerial { get; set; }
        public string Status { get; set; }
        public string ActiveBy { get; set; }
        public string CreatedBy { get; set; }
        public string DeleteBy { get; set; }
        public string Collectby { get; set; }
        public string PaymentBy { get; set; }
        public string Button { get; set; }
        //public int CollectBy { get; set; }
        public string ResellerType { get; set; }
        public string ResellerLogoPath { get; set; }
        public string PaymentTime { get; set; }
        public double LastAmount { get; set; }
        public DateTime PaymenReceivedDate { get; set; }
    }
}