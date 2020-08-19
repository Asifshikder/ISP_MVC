using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("37")]
    public class StockDetails
    {
        [Key]
        //[Column("A")]
        public int StockDetailsID { get; set; }
        //[Column("AQ")]
        public int StockID { get; set; }
        public virtual  Stock Stock { get; set; }
        //[Column("AW")]
        public int ?BrandID { get; set; }
        public virtual Brand Brand { get; set; }
        //[Column("AE")]
        public int SectionID { get; set; }

        public virtual Section Section { get; set; }
        //[Column("AR")]
        public int ?SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }
        //[Column("AZ")]
        public string SupplierInvoice { get; set; }
        //[Column("AX")]
        public string Serial { get;set; }
        //[Column("AC")]
        public string BarCode { get; set; }
        //[Column("AV")]
        public int ProductStatusID { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
        //[Column("WP")]
        public bool WarrentyProduct { get; set; }
        //[Column("AB")]
        public string CreatedBy { get; set; }
        //[Column("AN")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AM")]
        public string UpdateBy { get; set; }
        //[Column("APO")]
        public DateTime? UpdateDate { get; set; }
    }
}