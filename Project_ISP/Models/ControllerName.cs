using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("z")]
    public class ControllerName
    {

        [Key]
        //[Column("AM")]
        public int ControllerNameID { get; set; }
        //[Column("AH")]
        public string ControllerNames { get; set; }
        //[Column("AG")]
        public string ControllerValue { get; set; }
        //[Column("AF")]
        public string CreateBy { get; set; }
        //[Column("AD")]
        public DateTime CreateDate { get; set; }
        //[Column("AS")]
        public string UpdateBy { get; set; }
        //[Column("AA")]
        public DateTime? UpdateDate { get; set; }
        //[Column("A")]
        public int ShowingStatus { get; set; }

    }
}