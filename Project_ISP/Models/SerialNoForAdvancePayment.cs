using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{

    //[Table("33")]
    public class SerialNoForAdvancePayment
    {
        [Key]
        //[Column("A")]
        public int SerialNoForAdvancePaymentID { get; set; }
        //[Column("ASS")]
        public int SerialNumber { get; set; }
    }
}