using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class MikrotikUserCutomModel
    {
        public int ClientDetailsID { get; set; }
        public int MikrotikID { get; set; }
        public string ClientName { get; set; }
        public string LoginName { get; set; }
        public string PackageName { get; set; }
        public string Zone { get; set; }
        public string ContactNumber { get; set; }
        public string MikrotikName { get; set; }
        public int IPPoolID { get; set; }
        public string PoolName { get; set; }
        public string LocalAddress { get; set; }
        public bool UpdateMikrotikUserInformation { get; set; }
    }
}