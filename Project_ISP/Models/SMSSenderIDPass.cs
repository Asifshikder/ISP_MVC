using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("35")]
    public class SMSSenderIDPass
    {[Key]
        //[Column("A")]
        public int SMSSenderIDPassID { get; set; }
        //[Column("AA")]
        public string ID { get; set; }
        //[Column("AS")]
        public string Pass { get; set; }
        //[Column("AD")]
        public string Sender { get; set; }
        //[Column("AF")]
        public string CompanyName { get; set; }
        //[Column("GA")]
        public int Status { get; set; }
        //[Column("AH")]
        public string HelpLine { get; set; }
        //[Column("AJ")]
        public string CreateBy { get; set; }
        //[Column("AK")]
        public DateTime CreateDate { get; set; }
        //[Column("ALQ")]
        public string UpdateBy { get; set; }
        //[Column("AAAQWA")]
        public DateTime ? UpdateDate { get; set; }
    }
}