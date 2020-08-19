using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("21")]
    public class Package
    {
        [Key]
        //[Column("AP")]
        public int PackageID { get; set; }
        //[Column("AO")]
        public int? IPPoolID { get; set; }
        public virtual IPPool IpPool { get; set; }
        //[Column("AI")]
        public int? MikrotikID { get; set; }
        public virtual Mikrotik Mikrotik { get; set; }
        //[Column("AU")]
        public string LocalAddress { get; set; }
        //[Column("AY")]
        public string PackageName { get; set; }
        //[Column("AT")]
        public string OldPackageName { get; set; }
        //[Column("AR")]
        public float PackagePrice { get; set; }
        //[Column("AE")]
        public string BandWith { get; set; }
        //[Column("AW")]
        public string CreatedBy { get; set; }
        //[Column("AS")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AA")]
        public string UpdateBy { get; set; }
        //[Column("A")]
        public DateTime? UpdateDate { get; set; }


        public int PackageForMyOrResellerUser { get; set; } 
        public int Status { get; set; }
    }
}