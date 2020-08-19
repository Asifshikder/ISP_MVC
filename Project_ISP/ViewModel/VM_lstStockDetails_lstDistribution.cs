using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP_ManagementSystemModel.Models;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_lstStockDetails_lstDistribution
    {
        public List<StockDetails> lstStockDetails { get; set; }
        public List<Distribution> lstDistribution { get; set; }
    }
}