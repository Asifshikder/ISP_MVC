using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel.CustomClass
{
    public class CustomPurchaseList
    {
        public int PID { get; set; }
        public string InvoiceID { get; set; }
        public string AccountName { get; set; }
        public double Amount { get; set; }
        public double PurchasePayment { get; set; }
        public string IssuedAt { get; set; }
        public string ProductStatus { get; set; }
        public string TableStatus { get; set; }
        public string Type { get; set; }
        public bool PurchaseUpdate { get; set; }



        public double TotalAmount { get; set; }
        public double TotalPaidAmount { get; set; }
        public double TotalUnPaidAmount { get; set; } 
        public double TotalCancelAmount { get; set; }
        public double PaidPercent { get; set; }
        public double UnPaidPercent { get; set; }
        public double PartiallyPaidPercent { get; set; }
        public double CancelPercent { get; set; }
        public string Button { get; set; }
    }
}