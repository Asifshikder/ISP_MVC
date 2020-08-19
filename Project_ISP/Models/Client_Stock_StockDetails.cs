using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("m")]
    public class Client_Stock_StockDetails
    {
        //[Column("AQQQ")]
        public int StockID { get; set; }
        //[Column("AF")]
        public int StockDetailsID { get; set; }
        //[Column("AZ")]
        public int ItemID { get; set; }
        //[Column("AR")]
        public string ItemName { get; set; }
        //[Column("AE")]
        public int BrandID { get; set; }
        //[Column("AW")]
        public string BrandName { get; set; }
        //[Column("AQ")]
        public int SupplierID { get; set; }
        //[Column("AF")]
        public string SupplierName { get; set; }
        //[Column("AD")]
        public string SupplierInvoice { get; set; }
        //[Column("AS")]
        public string Serial { get; set; }
        //[Column("WP")]
        public bool WarrentyProduct { get; set; }

    }
}