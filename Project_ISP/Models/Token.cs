using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("39")]
    public class Token
    {
        //[Column("A")]
        public int TokenID { get; set; }
        //[Column("AS")]
        public int TokenNumber { get; set; }
    }
}