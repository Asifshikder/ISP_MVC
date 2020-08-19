using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class ClientSearch
    {
        public int ClientDetailsID { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string PackageName { get; set; }
        public string PackageNameThisMonth { get; set; }
        public string PackageNameNextMonth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ZoneName { get; set; }
        public string ContactNumber { get; set; }
        public string StatusThisMonthID { get; set; }
        public string StatusNextMonthID { get; set; }
    }
}