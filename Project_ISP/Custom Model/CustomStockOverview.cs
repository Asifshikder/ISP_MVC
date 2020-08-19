using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    public class CustomStockOverview
    {
        public int StockID { get; set; }
        public string ItemName { get; set; }
        public int TotalItemCount { get; set; }

        public int ProductInStock { get; set; }
        public int ProductInRunning { get; set; }
        public int ProductInRepair { get; set; }
        public int ProductInWarrenty { get; set; }
        public int ProductInDead { get; set; }
    }
}