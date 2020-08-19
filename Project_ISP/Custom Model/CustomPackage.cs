using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class CustomPackage
    {

        public int PackageID { get; set; }
        public string PackageName { get; set; }
        public string BandWith { get; set; }
        public string PackagePrice { get; set; }
        public int Client { get; set; }
        public string IPPoolName { get; set; }
        public string LocalAddress { get; set; }
        public string MikrotikName { get; set; }
        public bool PackageUpdate { get; set; }
    }
}