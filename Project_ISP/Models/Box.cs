using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("g")]
    public class Box
    {
        [Key]
        //[Column("A")]
        public int BoxID { get; set; }
        //[Column("AS")]
        public string BoxName { get; set; }
        public int? ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }
        //[Column("AD")]
        public string BoxLocation { get; set; }
        public string LatitudeLongitude { get; set; }
        //[Column("AF")]
        public string CreatedBy { get; set; }
        //[Column("AG")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AH")]
        public string UpdateBy { get; set; }
        //[Column("AJ")]
        public DateTime? UpdateDate { get; set; }
    }
}