using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    public class AccountListVsAmountTransfer
    {
        [Key]
        public int AccountListVsAmountTransferID { get; set; }
        public int FromAccountID { get; set; }//from account
        public int ToAccountID { get; set; }//To account
        public DateTime TransferDate { get; set; }
        public int CurrencyID { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public int PaymentByID { get; set; }
        public virtual PaymentBy PaymentBy { get; set; }
        public string References { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int BreakDownAccountListID { get; set; }// This is the ID for differentiate the In And Out
        public string TransferType { get; set; }//In Or out
    }

    //public class AccountListVsAmountTransfer
    //{
    //    [Key]
    //    public int AccountListVsAmountTransferID { get; set; }
    //    public int AccountListID { get; set; }
    //    public virtual AccountList AccountList { get; set; }
    //    public int ToAccountID { get; set; }
    //    public DateTime TransferDate { get; set; }
    //    public int CurrencyID { get; set; }
    //    public decimal Amount { get; set; }
    //    public string Description { get; set; }
    //    public string Tags { get; set; }
    //    public int PaymentByID { get; set; }
    //    public virtual PaymentBy PaymentBy{ get; set; }
    //    public string References { get; set; }
    //    public int Status { get; set; }
    //    public int CreateBy { get; set; }
    //    public DateTime CreateDate { get; set; }
    //    public int? UpdateBy { get; set; }
    //    public DateTime? UpdateDate { get; set; }
    //    public int? DeleteBy { get; set; }
    //    public DateTime? DeleteDate { get; set; }

    //}

}