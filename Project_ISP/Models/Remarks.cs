using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("27")]
    public class Remarks
    {
        [Key]
        //[Column("A")]
        public int RemarksID { get; set; }
        //[Column("AS")]
        public int RemarksNo { get; set; }
    }
}