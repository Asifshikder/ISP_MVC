using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("6")]
    public class Employee
    {
        [Key]
        //[Column("A")]
        public int EmployeeID { get; set; }
        //[Column("AS")]
        public string Name { get; set; }
        //[Column("AD")]
        public string LoginName { get; set; }
        //[Column("AF")]
        public string Password { get; set; }
        //[Column("AG")]
        public string Phone { get; set; }
        //[Column("AH")]
        public string Address { get; set; }
        //[Column("AJ")]
        public string Email { get; set; }
        //[Column("AK")]
        public int ? DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        //[Column("AL")]
        public int? RoleID { get; set; }
        public virtual Role Role { get; set; }
        //[Column("AQ")]
        public int EmployeeStatus { get; set; }
        //[Column("AW")]
        public string CreatedBy { get; set; }
        //[Column("AE")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AR")]
        public string UpdateBy { get; set; }
        //[Column("AT")]
        public DateTime? UpdateDate { get; set; }

        //[Column("ACCC")]
        public int? UserRightPermissionID { get; set; }
        public virtual UserRightPermission UserRightPermission { get; set; }


        public DateTime DOB { get; set; }
        public int DeviceID { get; set; }

        public int DutyShiftID { get; set; }
        public virtual DutyShift DutyShift { get; set; }
        public int Salary { get; set; }
        public int? DayID { get; set; }
        public virtual Day Days { get; set; }
        public int BreakHour { get; set; }
        public int BreakMinute { get; set; }
        public string DutyShiftCombined { get; set; }

        public byte[] EmployeeOwnImageBytes { get; set; }
        public string EmployeeOwnImageBytesPaths { get; set; }
        public byte[] EmployeeNIDImageBytes { get; set; }
        public string EmployeeNIDImageBytesPaths { get; set; }

        public int? ResellerID { get; set; }
        public Reseller Reseller { get; set; }

    }
}