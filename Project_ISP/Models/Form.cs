using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("9")]
    public class Form
    {
        [Key]
        //[Column("AHH")]
        public int FormID { get; set; }
        //[Column("AG")]
        public int ControllerNameID { get; set; }
        public virtual ControllerName ControllerName { get; set; }
        //[Column("AS")]
        public string FormName { get; set; }
        //[Column("DA")]
        public string FormValue { get; set; }
        public string FormDescription { get; set; }
        //[Column("AT")]
        public string FormLocation { get; set; }
        //[Column("AR")]
        public string CreateBy { get; set; }
        //[Column("AE")]
        public DateTime CreateDate { get; set; }
        //[Column("AW")]
        public string UpdateBy { get; set; }
        //[Column("AQ")]
        public DateTime? UpdateDate { get; set; }
        //[Column("A")]
        public int ShowingStatus { get; set; }
    }
}