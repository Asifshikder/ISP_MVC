using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    public class Deposit
    {
        public int DepositID { get; set; }
        public string Description { get; set; }
        public byte[] DescriptionFileByte { get; set; }
        public string DescriptionFilePath { get; set; }
        public decimal Amount { get; set; }
        public int HeadID { get; set; }
        public virtual Head Head { get; set; }
        public DateTime DepositDate { get; set; }
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public int AccountListID { get; set; }
        public virtual AccountList AccountList { get; set; }
        public int PayerID { get; set; }
        public virtual CompanyVSPayer CompanyVSPayer { get; set; }
        public int PaymentByID { get; set; }
        public virtual PaymentBy PaymentBy { get; set; }
        public int DepositStatus { get; set; }
        public string References { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}