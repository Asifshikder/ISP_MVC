using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("v")]
    public class ClientStockAssign
    {
        //[Column("A")]
        public int StockID { get; set; }
        //[Column("AS")]
        public int StockDetailsID { get; set; }
        //[Column("AD")]
        public int EmployeeID { get; set; }

    }
}