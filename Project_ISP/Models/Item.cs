using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("14")]
    public class Item
    {
        [Key]
        //[Column("SSA")]
        public int ItemID { get; set; }
        //[Column("tA")]
        public string ItemName { get; set; }
        //public int BrandID { get; set; }
        //public virtual Brand Brand { get; set; }
        //[Column("rrA")]
        public int? ItemFor { get; set; }
        public string ItemCode { get; set; }
        public string CreatedBy { get; set; }
        //[Column("Ae")]
        public DateTime? CreatedDate { get; set; }
        //[Column("Aw")]
        public string UpdateBy { get; set; }
        //[Column("A")]
        public DateTime? UpdateDate { get; set; }
    }
}