using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
   
    public class CableCustomList
    {
        public int CableStockID { get; set; }
        public string CableTypeName { get; set; }
        public string BoxDrumName { get; set; }
        public string BrandName { get; set; }
        public string SupplierName { get; set; }
        public string Invoice { get; set; }
        public int ReadingFrom { get; set; }
        public int ReadingEnd { get; set; }
        public double Quantity { get; set; }
        public double Used { get; set; }
        public double Remain { get; set; }
    }
}