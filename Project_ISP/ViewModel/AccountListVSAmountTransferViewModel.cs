using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class AccountListVSAmountTransferViewModel
    {
        public int AccountListVsAmountTransferID { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public DateTime TransferDate { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Tags { get; set; }
        public string PaymentBy { get; set; }
        public string References { get; set; }
        public bool UpdateTansfer { get; set; }

    }
}