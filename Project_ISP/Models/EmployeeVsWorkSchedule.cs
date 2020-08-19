using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class EmployeeVsWorkSchedule
    {
        [Key]
        public int EmployeeVsWorkScheduleID { get; set; }
        public int DayID { get; set; }
        public virtual Day Days { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int RunHour { get; set; }

        public int RunMinute { get; set; }
        public int BreakStartHour { get; set; }
        public int BreakStartMinute { get; set; }
        public int BreakEndHour { get; set; }
        public int BreakEndMinute { get; set; }
        public int EmployeeID { get; set; }
        public virtual Employee Employees { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}