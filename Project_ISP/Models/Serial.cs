using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    //[Table("32")]
    public class Serial
    {
        [Key]
        //[Column("A")]
        public int SerialID { get; set; }
        //[Column("AS")]
        public int SerialNo { get; set; }

    }
}