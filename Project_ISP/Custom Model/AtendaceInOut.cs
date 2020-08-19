using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class AtendaceInOut
    {
        [Key]
        public int AttendaceInOutID { get; set; }
        public string Title { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public decimal InSalaryCut { get; set; }
        public decimal OutSalaryCut { get; set; }
        public int AttendanceTypeID { get; set; }
        public virtual AttendanceType AttendanceType { get; set; }
        public int Status { get; set; }
    }
}