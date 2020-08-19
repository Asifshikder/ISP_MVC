using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations.Schema;
using Project_ISP.Models;

namespace ISP_ManagementSystemModel.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }
        public string Descriptions { get; set; }
        public byte[] DescriptionFileByte { get; set; }
        public string DescriptionFilePath { get; set; }
        public int HeadID { get; set; }
        public virtual Head Head { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
         public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public int AccountListID { get; set; }
        public virtual AccountList AccountList { get; set; }
        public int PayerID { get; set; }
        public virtual CompanyVSPayer CompanyVSPayer { get; set; }
        public int PaymentByID { get; set; }
        public virtual PaymentBy PaymentBy { get; set; }
        public int ExpenseStatus { get; set; }
        public string References { get; set; }
        public int ResellerID { get; set; }
        public Reseller Reseller { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}