using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("36")]
    public class Stock
    {
        [Key]
        //[Column("A")]
        public int StockID { get; set; }
       //// //[Column("AS")]
        public int ItemID { get; set; }
        public virtual Item Item { get; set; }
        //[Column("AD")]
        public int? Quantity { get; set; }
        //[Column("AFFF")]
        public  int ? UsedStatus { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public string UpdateBy { get; set; }
        //public DateTime? UpdateDate { get; set; }
    }
}