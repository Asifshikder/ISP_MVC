using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("x")]
    public class Complain
    {
        //[Column("AZZZ")]
        public int ComplainID { get; set; }
        //[Column("AP")]
        public int ClientDetailsID { get; set; }//client details
        public virtual ClientDetails ClientDetails { get; set; }
        //[Column("AO")]
        public int TokenNo { get; set; }
        //[Column("AOO")]
        public string MonthlySerialNo { get; set; }
        //[Column("AI")]
        public string ComplainDetails { get; set; }
        //[Column("AU")]
        public int? EmployeeID { get; set; }//Assign to

        public virtual Employee Employee { get; set; }
        public int? ResellerID { get; set; }

        public virtual Reseller Reseller { get; set; }
        //[Column("AY")]
        public int LineStatusID { get; set; }// for Status
        public virtual LineStatus LineStatus { get; set; }
        //[Column("AT")]
        public int ComplainTypeID { get; set; }
        public virtual ComplainType ComplainType { get; set; }
        //[Column("")]
        public string WhichOrWhere { get; set; }
        //[Column("AR")]
        public int ComplainOpenBy { get; set; }
        //[Column("AE")]
        public DateTime ComplainTime { get; set; }

        public bool OnRequest { get; set; }

        //[Column("AW")]
        public string UpdateBy { get; set; }
        //[Column("AQ")]
        public DateTime? UpdateDate { get; set; }
    }
}