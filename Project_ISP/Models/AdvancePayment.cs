using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{

    //[Table("c1")]
    public class AdvancePayment
    {
        [Key]
        //[Column("A")]
        public int AdvancePaymentID { get; set; }
        //[Column("Aqd")]
        public int ClientDetailsID { get; set; }

        public virtual ClientDetails ClientDetils { get; set; }
        //[Column("Aa")]
        public double AdvanceAmount { get; set; }
        //[Column("As")]
        public string Remarks { get; set; }
        //[Column("Ad")]
        public string CollectBy { get; set; }
        //[Column("AqS")]
        public string CreatePaymentBy { get; set; }
        //[Column("Add")]
        public DateTime FirstPaymentDate { get; set; }
        //[Column("Aq")]
        public string UpdatePaymentBy { get; set; }
        //[Column("Af")]
        public DateTime? UpdatePaymentDate { get; set; }

    }
}