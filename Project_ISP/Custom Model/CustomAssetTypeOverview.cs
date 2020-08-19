using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class CustomAssetTypeOverview
    {
        public int AssetTypeID { get; set; }
        public string AssetTypeName { get; set; }
        public int TotalAssetTypeCount { get; set; }
        
    }
}