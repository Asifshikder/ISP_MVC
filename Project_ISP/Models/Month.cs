using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("19")]
    public class Month
    {
        [Key]
        //[Column("A")]
        public int MonthID { get; set; }
        //[Column("AD")]
        public string MonthName { get; set; }

    }
}