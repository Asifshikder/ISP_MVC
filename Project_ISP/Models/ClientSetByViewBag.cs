using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("u")]
    public class ClientSetByViewBag
    {
        //[Column("A")]
        public int ClientDetailsID { get; set; }
        //[Column("AA")]
        public int TransactionID { get; set; }
        //[Column("AS")]
        public float PaymentAmount { get; set; }

        public string LineStatusActiveDate { get; set; }
        public ClientLineStatus ClientLineStatus { get; set; }
    }
}