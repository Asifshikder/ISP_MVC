using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("n")]
    public class Client_Stock_StockDetails_ForDistribution
    {
        //[Column("AFF")]
        public int StockID { get; set; }
        //[Column("AD")]
        public int StockDetailsID { get; set; }
        //[Column("AS")]
        public int ? PopID { get; set; }
        //[Column("AA")]
        public int ? BoxID { get; set; }
        //[Column("AM")]
        public int ? CustomerID { get; set; }
        //[Column("AN")]
        public int  EmployeeID { get; set; }
        //[Column("AB")]
        public int  DistributionReasonID { get; set; }
        //[Column("AV")]
        public int? OldStockID { get; set; }
        //[Column("AC")]
        public int? OldStockDetailsID { get; set; }
        //[Column("AX")]
        public int? OldSectionID { get; set; }
        //[Column("AZ")]
        public int? OldProductStatusID { get; set; }
        public string Remarks { get; set; }


    }
}