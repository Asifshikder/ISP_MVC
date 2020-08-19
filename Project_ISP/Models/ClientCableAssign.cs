using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("p")]
    public class ClientCableAssign
    {
        //[Column("Ad")]
        public int CableStockID { get; set; }
        //[Column("As")]
        public int CableQuantity { get; set; }
        //[Column("Aa")]
        public int EmployeeID { get; set; }
    }
}