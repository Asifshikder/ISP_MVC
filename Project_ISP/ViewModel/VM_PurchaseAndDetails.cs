using Project_ISP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_PurchaseAndDetails
    {
        public Purchase purchase { get; set; }
        public List<PurchaseDeatils> purchasedeatils { get; set; }
    }
}