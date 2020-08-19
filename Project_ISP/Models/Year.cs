using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("42")]
    public class Year
    {
        [Key]
        //[Column("A")]
        public int YearID { get; set; }
        //[Column("AS")]
        public string YearName { get; set; }
    }
}