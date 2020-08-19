using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    public class AccountList
    {
        public int AccountListID { get; set; }
        public string AccountTitle { get; set; }
        public string Description { get; set; }
        public decimal? InitialBalance { get; set; }
        public int AccountNumber { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public  string BankUrl { get; set; }
        public int OwnerID { get; set; }
        public virtual  AccountOwner AccountOwner { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateData { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int Status { get; set; }
    }
}