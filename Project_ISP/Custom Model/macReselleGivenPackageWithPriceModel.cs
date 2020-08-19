using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class macReselleGivenPackageWithPriceModel
    {
        //PID : pID, PP : pp
        public int PID { get; set; }
        public string PName { get; set; }
        public double PPAdmin { get; set; }
        public double PPFromRS { get; set; }
        public int Status { get; set; }
    }
    
}