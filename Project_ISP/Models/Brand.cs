using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("h")]
    public class Brand
    {
        [Key]
        //[Column("A")]
        public int BrandID { get; set; }
        //[Column("AB")]
        public string BrandName { get; set; }
        //[Column("AC")]
        public string CreatedBy { get; set; }
        //[Column("AF")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AE")]
        public string UpdateBy { get; set; }
        //[Column("AW")]
        public DateTime? UpdateDate { get; set; }
    }
}