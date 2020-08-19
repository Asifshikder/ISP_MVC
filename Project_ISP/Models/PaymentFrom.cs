using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("22")]
    public class PaymentFrom
    {
        [Key]
        //[Column("A")]
        public int PaymentFromID{get;set;}
        //[Column("AS")]
        public string PaymentFromName { get; set; }
    }
}