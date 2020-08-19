using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ISP_ManagementSystemModel.Models;

namespace Project_ISP.Models
{
    [Table("PurchaseDeatils")]
    public class PurchaseDeatils
    {
        [Key]
        public int PurchaseDeatilsID { get; set; }
        public int PurchaseID { get; set; }
        public Purchase Purchase { get; set; }
        public int ItemID { get; set; }
        public Item Item { get; set; }
        //public Item
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public bool HasWarrenty { get; set; }
        public DateTime? WarrentyStart { get; set; }
        public DateTime? WarrentyEnd { get; set; }
        public string Serial { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}