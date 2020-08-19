using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using CrystalDecisions.CrystalReports.Engine;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    //[Table("C")]
    public class ComplainType
    {
        [Key]
        //[Column("A")]
        public int ComplainTypeID { get; set; }
        //[Column("AA")]
        public string ComplainTypeName { get; set; }
        //[Column("")]
        public bool ShowMessageBox { get; set; }
        //[Column("AS")]
        public string CreatedBy { get; set; }
        //[Column("AD")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AF")]
        public string UpdateBy { get; set; }
        //[Column("AG")]
        public DateTime? UpdateDate { get; set; }
        //[Column("AH")]
        public bool Status { get; set; }
    }
}