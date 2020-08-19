using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("w")]
    public class ClientUserPermission
    {
        //[Column("A")]
        public int UserRightPermissionID { get; set; }
        //[Column("B")]
        public string UserRightPermissionName { get; set; }
        
    }
}