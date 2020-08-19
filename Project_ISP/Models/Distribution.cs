using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.DynamicData;
using ISP_ManagementSystemModel.Models;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("3")]
    public class Distribution
    {
        [Key]
        //[Column("Aq")]
        public int DistributionID { get; set; }
        //[Column("Aw")]
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        //[Column("Ae")]
        public int StockDetailsID { get; set; }
        public virtual StockDetails StockDetails { get; set; }
        //[Column("Ar")]
        public int? PopID { get; set; }
        public virtual Pop Pop { get; set;}
        //[Column("Aa")]
        public int? BoxID { get; set; }
        public virtual Box Box { get; set; }
        //[Column("As")]
        public int? ClientDetailsID { get; set; }
        public virtual ClientDetails ClientDetails { get; set; }
        //[Column("Ad")]
        public int? DistributionReasonID { get; set; }
        public virtual DistributionReason DistributionReason { get; set; }
        //[Column("Af")]
        public int IndicatorStatus { get; set; }
        //[Column("Ag")]
        public string CreatedBy { get; set; }
        //[Column("Ah")]
        public DateTime? CreatedDate { get; set; }
        //[Column("Az")]
        public string UpdateBy { get; set; }
        //[Column("Ax")]
        public DateTime? UpdateDate { get; set; } 
        public string Remarks { get; set; }

    }
}