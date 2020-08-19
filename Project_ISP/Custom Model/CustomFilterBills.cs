using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class CustomFilterBills
    {
        public int TransactionID { get; set; }
        public int PaymentTypeID { get; set; }
        public int ClientDetailsID { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Zone { get; set; }
        public string PackageName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Amount { get; set; }
        public string PaymentAmountFor { get; set; }
        public string PaymentType { get; set; }
        public string PaidBy { get; set; }
        public DateTime PaidTime { get; set; }
      public bool IsPriorityClient { get; set; }
        public string ClientLoginName { get; set; }
        public string LineStatusActiveDate { get; set; }
    }
}