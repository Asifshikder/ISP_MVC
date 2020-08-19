using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel.CustomClass
{
    public class PurchaseCustomPaymentInformation
    {
        public int pphid { get; set; }  
        public string PaymentDate { get; set; }
        public string PaymentFrom { get; set; }
        public string PaymentMethod { get; set; }
        public double PaymentAmount { get; set; }
        public string CheckOrResetNo { get; set; }
        public string EntryBy { get; set; }
        public string Delete_By { get; set; }
        public string DeleteDate { get; set; }
        public string Button { get; set; }
        public string Status { get; set; }
    }
}