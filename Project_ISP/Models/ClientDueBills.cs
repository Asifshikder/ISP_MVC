using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("s")]
    public class ClientDueBills
    {
        [Key]
        //[Column("SDFA")]
        public int ClientDueBillsID { get; set; }
        //[Column("XA")]
        public int ClientDetailsID { get; set; }
        public virtual ClientDetails ClientDetails { get; set; }
        //[Column("YA")]
        public double DueAmount { get; set; }
        //[Column("TA")]
        public int Year { get; set; }
        //[Column("RA")]
        public int Month { get; set; }
        //[Column("EA")]
        public string CreateBy { get; set; }
        //[Column("WA")]
        public DateTime ?CreateDate { get; set; }
        //[Column("QA")]
        public string UpdateBy { get; set; }
        //[Column("AA")]
        public DateTime ?UpdateDate { get; set; }
        //[Column("A")]
        public bool Status { get; set; }
    }
}