using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("q")]
    public class ClientCableDistribution
    {
        //[Column("A")]
        public int CableStokID { get; set; }
        //[Column("AS")]
        public int CableQuantity { get; set; }
        //[Column("AD")]
        public int? EmployeeID { get; set; }
        //[Column("AT")]
        public int? ClientID { get; set; }
        //[Column("AU")]
        public int? AssignEmployee { get; set; }
    }
}