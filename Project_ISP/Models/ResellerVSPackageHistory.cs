using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    public class ResellerVSPackageHistory
    {
        [Key]
        public int ResellerVSPackageHistoryID { get; set; }
        public int ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }
        public int ResellerName { get; set; }
        public int ResellerPackageID { get; set; }
        //public virtual ResellerPackage ResellerPackage { get; set; }
        public string PackageName { get; set; }
        public string PackagePrice { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int DeleteBy { get; set; }
        public DateTime DeleteDate { get; set; }
    }
}