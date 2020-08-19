using Project_ISP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    [Table("PurchasePaymentHistory")]
    public class PurchasePaymentHistory
    {
        [Key]
        public int PurchasePaymentHistoryID { get; set; }
        public int PurchaseID { get; set; }
        public Purchase purchase { get; set; }
        public int AccountListID { get; set; }
        public AccountList AccountList { get; set; }
        public int PaymentByID { get; set; }
        //public PaymentBy PaymentBy { get; set; }
        public DateTime PurchasePaymentDate { get; set; }
        public string CheckNo { get; set; }
        public string CheckName { get; set; }
        public string CheckPath { get; set; }
        public byte[] CheckImageBytes { get; set; }
        public string Description { get; set; }
        public double PaymentAmount { get; set; }
        public int PaymentPaidBy { get; set; }//mean which employee paid the bill to that supplier
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool DeleteByParent { get; set; }
    }
}