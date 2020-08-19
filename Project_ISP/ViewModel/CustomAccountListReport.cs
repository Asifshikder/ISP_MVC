using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class CustomAccountListReport
    {
        public int ReportID { get; set; }
        public DateTime Date { get; set; }
        public string AccountListName { get; set; }
        public string transactionType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Button { get; set; }

    }
}