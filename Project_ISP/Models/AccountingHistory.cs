using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    [Table("AccountingHistory")]
    public class AccountingHistory
    {
        [Key]
        public int AccountingHistoryID { get; set; }
        public int AccountListID{ get; set; }
        public int PurchaseID { get; set; }
        public int SalesID { get; set; }
        public int DepositID { get; set; }
        public int ExpenseID { get; set; }
        public int ActionTypeID { get; set; }//what type of action it is.
        public int DRCRTypeID { get; set; }//is it DR Or CR
        public double Amount { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int DeleteBy { get; set; }
        public DateTime? DeteDate { get; set; }

    }
}