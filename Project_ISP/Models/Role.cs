using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("29")]
    public class Role
    {
        [Key]
        //[Column("A")]
        public int RoleID { get; set; }
        //[Column("AA")]
        public string RoleNae { get; set; }
        //[Column("AS")]
        public string CreatedBy { get; set; }
        //[Column("AD")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AFF")]
        public string UpdateBy { get; set; }
        //[Column("AG")]
        public DateTime? UpdateDate { get; set; }

    }
}