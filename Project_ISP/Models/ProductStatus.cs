using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("25")]
    public class ProductStatus
    {
        [Key]
        //[Column("A")]
        public int ProductStatusID { get; set; }
        //[Column("AS")]
        public string ProductStatusName { get; set; }
        //[Column("AD")]
        public string CreatedBy { get; set; }
        //[Column("AF")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AG")]
        public string UpdateBy { get; set; }
        //[Column("AH")]
        public DateTime? UpdateDate { get; set; }
    }
}