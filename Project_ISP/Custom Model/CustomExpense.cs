using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class CustomExpense
    {
        public int ExpenseID { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public string PaidTo { get; set; }
        public string PaidBy { get; set; }
        public string Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public bool UpdateExpense { get; set; }
        public bool DeleteExpense  {  get; set; }
    }
}