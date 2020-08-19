using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class DepositViewModel
    {
        public int DepositID { get; set; }
        public string Description { get; set; }
        public string DescriptionFilePath { get; set; }
        public decimal Amount { get; set; }
        public string HeadName { get; set; }
        public DateTime DepositDate { get; set; }
        public string  CompanyName { get; set; }
        public string  PaymentBy { get; set; }
        public int Status { get; set; }
        public bool UpdateDeposit { get; set; }
    }
}