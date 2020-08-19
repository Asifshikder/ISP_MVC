using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("5")]
    public class DistributionReason
    {
        [Key]
        //[Column("A")]
        public int DistributionReasonID { get; set; }
        //[Column("AA")]
        public string DistributionReasonName { get; set; }
        //[Column("AS")]
        public string CreatedBy { get; set; }
        //[Column("AD")]
        public DateTime? CreatedDate { get; set; }
        //[Column("DA")]
        public string UpdateBy { get; set; }
        //[Column("AAA")]
        public DateTime? UpdateDate { get; set; }
    }
}