using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class CustomStockListInformation
    {
        public int StockID { get; set; }
        public int StockDetailsID { get; set; }
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public string Serial { get; set; }
        public string SectionName { get; set; }
        public string ProductStatusName { get; set; }
        public bool DeleteStockList { get;set;}
    }
}