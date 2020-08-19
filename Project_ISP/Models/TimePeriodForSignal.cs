using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    //[Table("tpfs")]
    public class TimePeriodForSignal
    {
        [Key]
        public int TimePeriodForSignalID { get; set; }
        public double UpToHours { get; set; }
        public int SignalSign { get; set; }
        //[Column("AC")]
        public string CreatedBy { get; set; }
        //[Column("AF")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AE")]
        public string UpdateBy { get; set; }
        //[Column("AW")]
        public DateTime? UpdateDate { get; set; }
    }
}