using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.Custom_Model
{
    public class ResellerCustomInforamation
    {
        public int ResellerID { get; set; } 
        public string ResellerName { get; set; } 
        public string ResellerLoginName { get; set; } 
        public string ResellerPassword { get; set; } 
        public string ResellerTypeID { get; set; }
        public string ResellerTypeNameList { get; set; }
        public string ResellerAddress { get; set; } 
        public string ResellerContact { get; set; } 
        public string ResellerBillingCycleList { get; set; } 
        public int ResellerStatus { get; set; } 
        public bool ShowButton { get; set; }
        public string MacResellerAssignMikrotik { get; set; }
        public string BandwithResellerPackage { get; set; }
        public string MacResellerPackage { get; set; }
        public string CurrentBalance { get; set; }
    }
}