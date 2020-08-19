using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class Day
    {
        [Key]
        public int DayID { get; set; }
        public string DayName { get; set; }
    }
}