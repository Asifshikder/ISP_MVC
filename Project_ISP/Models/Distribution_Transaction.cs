using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{

    //[Table("4")]
    public class Distribution_Transaction
    {
        //[Column("A")]
        public int StockDetailsID { get; set; }
        //[Column("AQ")]
        public int SectionID { get; set; }
        //[Column("AW")]
        public int ProductStatusID { get; set; }
        //[Column("AE")]
        public string ItemName { get; set; }
        //[Column("AR")]
        public string BrandName { get; set; }
        //[Column("AT")]
        public string Serial { get; set; }
        //[Column("AY")]
        public string ClientName { get; set; }
        //[Column("AU")]
        public string EmployeeName { get; set; }
        //[Column("AZ")]
        public string SectionName { get; set; }
        //[Column("AV")]
        public string ProductStatus { get; set; }

    }
}