using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("15")]
    public class LineStatus
    {
        [Key]
        //[Column("A")]
        public int LineStatusID { get; set; }
        //[Column("AB")]
        public string LineStatusName { get; set; }
    }
}