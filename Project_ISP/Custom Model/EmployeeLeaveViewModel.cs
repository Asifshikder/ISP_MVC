using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Custom_Model
{
    public class EmployeeLeaveViewModel
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string LoginName { get; set; }
        public string Reason { get; set; }
        public int LeaveTypeID { get; set; }
        public string LeaveTypeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}