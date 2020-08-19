using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class TimePeriodForSignalCustomList
    {
        public int TimePeriodForSignalID { get; set; }
        public double UpToHours { get; set; }
        public int SignalSign { get; set; }
        public string SignalSignString { get; set; }
        public bool Button { get; set; }
    }
}