using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class EmployeeLeaveHistory
    {
        [Key]
        public int EmployeeLeaveHistoryID { get; set; }
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        public string Reason { get; set; }
        public int LeaveType { get; set; }
        public virtual LeaveSallaryType LeaveTypes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
}