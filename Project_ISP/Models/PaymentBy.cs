using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    //[Table("PB")]
    public class PaymentBy
    {
        [Key]
        //[Column("1")]
        public int PaymentByID { get; set; }
        //[Column("2")]
        public string PaymentByName { get; set; }
        //[Column("3")]
        public string CreatedBy { get; set; }
        //[Column("4")]
        public DateTime? CreatedDate { get; set; }
        //[Column("5")]
        public string UpdateBy { get; set; }
        //[Column("6")]
        public DateTime? UpdateDate { get; set; }
        //[Column("7")]
        public int Status { get; set; }
    }
}