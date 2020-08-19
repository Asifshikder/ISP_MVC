using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("41")]
    public class UserRightPermission
    {
        [Key]
        //[Column("AFF")]
        public int UserRightPermissionID { get; set; }
        //[Column("DDA")]
        public string UserRightPermissionName { get; set; }
        //[Column("AT")]
        public string UserRightPermissionDescription { get; set; }
        //[Column("AR")]
        public string UserRightPermissionDetails { get; set; }
        //[Column("EA")]
        public string CreateBy { get; set; }
        //[Column("AW")]
        public DateTime CreateDate { get; set; }
        //[Column("AQ")]
        public string UpdateBy { get; set; }
        //[Column("A")]
        public DateTime? UpdateDate { get; set; }
    }
}