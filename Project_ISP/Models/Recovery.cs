using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using ISP_ManagementSystemModel.Models;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("26")]
    public class Recovery
    {
        [Key]
        //[Column("A")]
        public int RecoveryID { get; set; }
        //[Column("AQ")]
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        //[Column("AW")]
        public int DistributionReasonID { get; set; }
        public virtual DistributionReason DistributionReason { get; set; }
        //[Column("AE")]
        public int DistributionID { get; set; }
        public virtual Distribution Distribution { get; set; }
        //[Column("AR")]
        public int StockDetailsID { get; set; }
        public virtual StockDetails StockDetails { get; set; }
        //[Column("AT")]
        public int? PopID { get; set; }

        public virtual Pop Pop { get; set; }
        //[Column("AA")]
        public int? BoxID { get; set; }
        public virtual Box Box { get; set; }
        //[Column("AS")]
        public int? ClientDetailsID { get; set; }
        public virtual ClientDetails ClientDetails { get; set; }
        //[Column("AD")]
        public DateTime RecoveryDate { get; set; }
        //[Column("AF")]
        public int IndicatorStatus { get; set; }
        //[Column("AG")]
        public string CreatedBy { get; set; }
        //[Column("AZ")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AX")]
        public string UpdateBy { get; set; }
        //[Column("AC")]
        public DateTime? UpdateDate { get; set; }
    }
}