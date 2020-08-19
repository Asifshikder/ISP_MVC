using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class AttendanceInOutViewModel
    {
        public int id { get; set; }
        public string Title { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public decimal InsalaryCut { get; set; }
        public decimal OutSalaryCut { get; set; }
        public int AtendanceType { get; set; }
        public int typeId { get; set; }
        public int TableStatusID { get; set; }

    }
}