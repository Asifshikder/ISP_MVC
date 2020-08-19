using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    [Table("Purchase")]
    public class Purchase
    {
        [Key]
        public int PurchaseID { get; set; }
        public string Subject { get; set; }
        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }
        public int PublishStatus { get; set; }
        public string InvoicePrefix { get; set; }
        public string InvoiceID { get; set; }
        public DateTime IssuedAt { get; set; }
        public string SupplierNoted { get; set; }
        public double SubTotal { get; set; }
        public int DiscountType { get; set; }
        public double DiscountPercentOrFixedAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
        public double PurchasePayment { get; set; }
        public int? ResellerID { get; set; } 
        public int PurchaseStatus { get; set; }
        public int Status { get; set; }
        public Reseller Reseller { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}