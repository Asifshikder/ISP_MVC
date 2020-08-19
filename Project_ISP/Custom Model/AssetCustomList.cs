using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class AssetCustomList
    {
        public int AssetID { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetName { get; set; }
        public double AssetValue { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? WarrentyStartDate { get; set; }
        public DateTime? WarrentyEndDate { get; set; }
        public bool Button { get; set; }
    }
}