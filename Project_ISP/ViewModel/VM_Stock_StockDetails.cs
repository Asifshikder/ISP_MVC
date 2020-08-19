using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP_ManagementSystemModel.Models;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_Stock_StockDetails
    {
        public Stock Stock { get; set; }
        public List<StockDetails> LstStockDetails { get; set; }
    }
}