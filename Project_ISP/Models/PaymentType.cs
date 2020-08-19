using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("23")]
    public class PaymentType
    {
        [Key]
        //[Column("A")]
        public int PaymentTypeID { get; set; }
        //[Column("AS")]
        public string PaymentTypeName { get; set; }
    }
}