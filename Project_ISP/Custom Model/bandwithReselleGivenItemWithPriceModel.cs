using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class bandwithReselleGivenItemWithPriceModel
    { 
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public double ItemQuantity { get; set; }
        public double ItemTotalPrice { get; set; }
    }
}