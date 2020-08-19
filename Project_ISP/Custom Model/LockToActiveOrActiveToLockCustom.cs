using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class LockToActiveOrActiveToLockCustom
    {
        public int TransactionID { get; set; }
        public int ClientDetailsID { get; set; }
        public int ClientLineStatusID { get; set; }
        public string Name { get; set; }
        public string ClientLoginName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Zone { get; set; }
        public string PackageName { get; set; }
        public string PackagePrice { get; set; }
        public string EmployeeName { get; set; }
        public DateTime LineStatusChangeDate { get; set; }
        public bool IsPriorityClient { get; set; } 
        public string LineStatusActiveDate { get; set; }
    }
}