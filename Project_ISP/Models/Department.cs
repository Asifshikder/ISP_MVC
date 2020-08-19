using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("1")]
    public class Department
    {
        [Key]
        //[Column("A")]

        public int DepartmentID { get; set; }
        //[Column("AA")]
        public string DepartmentName { get; set; }
    }
}