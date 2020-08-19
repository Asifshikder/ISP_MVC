using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_ISP
{
    //[Table("RPL")]
    public class ResellerVSPayment
    {
        [Key]
        public int ResellerVSPaymentID { get; set; } 
    }
}