using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    //[Table("38")]
    public class Supplier
    {
        [Key]
        //[Column("A")]
        public int SupplierID { get; set; }
        //[Column("AA")]
        public string SupplierName { get; set; }
        //[Column("AS")]
        public string SupplierAddress { get; set; }
        //[Column("AD")]
        public string CreatedBy { get; set; }
        //[Column("AF")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AG")]
        public string UpdateBy { get; set; }
        //[Column("AH")]
        public DateTime? UpdateDate { get; set; }
    }
}