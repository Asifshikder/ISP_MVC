using ISP_ManagementSystemModel.ViewModel.CustomClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class VM_CustomPurchaseList_Percentage
    {
        public List<CustomPurchaseList> CustomPurchaseList { get; set; }

        public double PaidPercent { get; set; }
        public double UnPaidPercent { get; set; }
        public double PartiallyPaidPercent { get; set; }
        public double CancelPercent { get; set; }

    }
}