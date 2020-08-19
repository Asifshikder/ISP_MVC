using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class ResellerBillingCycle
    {
        [Key]
        //[Column("RBCD")]
        public int ResellerBillingCycleID { get; set; }
        //[Column("DY")]
        public int Day { get; set; }
        public int Status { get; set; }
        //[Column("CB")]
        public string CreatedBy { get; set; }
        //[Column("CD")]
        public DateTime? CreatedDate { get; set; }
        //[Column("UB")]
        public string UpdateBy { get; set; }
        //[Column("UD")]
        public DateTime? UpdateDate { get; set; }
        //[Column("DB")]
        public string DeleteBy { get; set; }
        //[Column("DD")]
        public DateTime? DeleteDate { get; set; }
    }
}