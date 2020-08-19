using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{

    //[Table("t")]
    public class ClientLineStatus
    {
        [Key]
        //[Column("AVC")]
        public int ClientLineStatusID { get; set; }
        //[Column("AZZ")]
        public int ClientDetailsID { get; set; }

        public virtual ClientDetails ClientDetails { get; set; }
        //[Column("AG")]
        public int? PackageID { get; set; } 
        public virtual Package Package { get; set; }
        //[Column("AAA")]
        public int LineStatusID { get; set; }
        public virtual LineStatus LineStatus { get; set; }
        //[Column("AQ")]
        public int ? LineStatusFromWhichMonth { get; set; }

        [Display(Name="Reason")]
        //[Column("AGSSS")]
        public string StatusChangeReason { get; set; }
        //[Column("AF")]
        public DateTime? LineStatusChangeDate { get; set; }
        //[Column("AD")]
        public int? EmployeeID { get; set; }
        public virtual Employee Employee { get; set; } 
        public int? ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }
        //[Column("AS")]
        public DateTime? CreateDate { get; set; }
        //[Column("A")]
        public int? MikrotikID { get; set; }
        public virtual Mikrotik Mikrotik { get; set; }


        //[Column("LineStatusAppliedOrNot")]
        public bool IsLineStatusApplied { get; set; }
       // //[Column("LineStatusActiveDates")]
        public DateTime? LineStatusWillActiveInThisDate { get; set; }
        //[Column("StatusFromNow")]
        public bool StatusFromNow { get; set; }




        public int StatusThisMonth { get; set; }
        public int StatusNextMonth { get; set; }

        public int PackageThisMonth { get; set; }
        public int PackageNextMonth { get; set; }
    }

}