using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    ////[Table("BGH")]
    public class BillGenerateHistory
    {
        [Key]
        ////[Column("A")]
        public int BIllGenerateHistoryID { get; set; }
        ////[Column("AS")]
        public string Year { get; set; }
        ////[Column("AD")]
        public string Month { get; set; }
        ////[Column("AF")]
        public string CreatedBy { get; set; }
        ////[Column("AG")]
        public DateTime? CreatedDate { get; set; }
        ////[Column("AH")]
        public string UpdateBy { get; set; }
        ////[Column("AJ")]
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
    }
}
