using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP_ManagementSystemModel.Models;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_Mikrotik_ClientPackage_ID
    {
        public int ClientDetailsID { get;set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public int ClientLineStatusID { get; set; }
        public int LineStatusNextMonth { get; set; }
        public int PackageNextMonth { get; set; }
        public int PackageThisMonth { get; set; }
        public string PackageName { get; set; }
        public Mikrotik ClientDetailsMikrotik { get;set; }
        //public Mikrotik ClientLineStatusMikrotik { get;set; }
    }
}