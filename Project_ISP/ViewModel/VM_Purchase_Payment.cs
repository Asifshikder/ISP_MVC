using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_Purchase_Payment
    {
        public double PaidAmount { get; set; }
        public double TotalAmount { get; set; }
        public double SubTotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double PurchasePayment { get; set; }
        public double DueAmount { get; set; }
        public string Payee { get; set; }
    }
}