using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("a")]
    public class ActionList
    {
        [Key]
        //[Column("AD")]
        public int ActionListID { get; set; }
        //[Column("A")]
        public int FormID { get; set; }
        public virtual  Form Form{ get; set; }
        //[Column("B")]
        public string ActionName { get; set; }
        //[Column("C")]
        public string ActionValue { get; set; }
        //[Column("D")]
        public string ActionDescription { get; set; }
        //[Column("E")]
        public string CreateBy { get; set; }
        //[Column("F")]
        public DateTime CreateDate { get; set; }
        //[Column("G")]
        public string UpdateBy { get; set; }
        //[Column("H")]
        public DateTime? UpdateDate { get; set; }
        //[Column("I")]
        public int ShowingStatus { get; set; }
    }
}