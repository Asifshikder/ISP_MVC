using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{

    //[Table("j")]
    public class CableStock
    {
        [Key]
        //[Column("AAA")]
        public int CableStockID { get; set; }
        //[Column("AH")]
        public int CableTypeID { get; set; }
        public virtual CableType CableType { get; set; }


        //[Column("BR1")]
        public int? BrandID { get; set; }
        public virtual Brand Brand { get; set; }
        //[Column("SPL1")]
        public int? SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }
        //[Column("AZ")]
        public string SupplierInvoice { get; set; }
        //[Column("AZ1")]
        public int FromReading { get; set; }
        //[Column("AZ2")]
        public int ToReading { get; set; }



        //[Column("AG")]
        public int CableUnitID { get; set; }
        public virtual CableUnit CableUnit { get; set; }
        //[Column("AF")]
        public string CableBoxName { get; set; }
        //[Column("AD")]
        public int CableQuantity { get; set; }
        //[Column("AS")]
        public int UsedQuantityFromThisBox { get; set; }
        //[Column("AA")]
        public int? TotallyUsed { get; set; }
        //this is the employeeid is who insert cable  in system
        //[Column("AY")]
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        //[Column("AT")]
        public int IndicatorStatus { get; set; }
        //[Column("AR")]
        public string CreatedBy { get; set; }
        //[Column("AE")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AW")]
        public string UpdateBy { get; set; }
        //[Column("AQ")]
        public DateTime? UpdateDate { get; set; }
    }
}