using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("13")]
    public class ISPAccessList
    {
        [Key]
        //[Column("AT")]
        public int ISPAccessListID { get; set; }
        //[Column("AR")]
        public string AccessName { get; set; }
        //[Column("AE")]
        public int AccessValue { get; set; }
        //[Column("AW")]
        public string CreateBy { get; set; }
        //[Column("AQ")]
        public DateTime CreateDate { get; set; }
        //[Column("AD")]
        public string UpdateBy { get; set; }
        //[Column("AS")]
        public DateTime? UpdateDate { get; set; }
        //[Column("AZ")]
        public int ShowingStatus { get; set; }
        //[Column("A")]
        public bool IsGranted { get; set; }
    }
}