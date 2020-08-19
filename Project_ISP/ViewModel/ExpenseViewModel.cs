using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class ExpenseViewModel
    {
        public int ExpenseID { get; set; }
        public string Description { get; set; }
        public string DescriptionFilePath { get; set; }
        public double Amount { get; set; }
        public string HeadName { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string CompanyName { get; set; }
        public string PaymentBy { get; set; }
        public int Status { get; set; }
        public bool UpdateExpense { get; set; }
    }
}