using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    public class ClientCustomInformation
    {
        public int ClientDetailsID { get; set; }
        public string chkSMS { get; set; }
        public int ClientLineStatusID { get; set; }
        public string ClientLineStatusName { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string PackageName { get; set; }
        public string PackageNameThisMonth { get; set; }
        public string PackageNameNextMonth { get; set; }
        public string Address { get; set; }
        public string LatitudeLongitude { get; set; }
        public string Email { get; set; }
        public string Zone { get; set; }
        public string ContactNumber { get; set; }
        public string StatusThisMonthID { get; set; }
        public string StatusNextMonthID { get; set; }
        public string StatusThisMonthName { get; set; }
        public string StatusNextMonthName { get; set; }
        public bool IsPriorityClient { get; set; }
        public string Show { get; set; }
        public bool Button { get; set; }
        public string LineStatusActiveDate { get; set; }

        public string TransactionID { get; set; }
        public string ProfileStatusUpdateInPercent { get; set; }
        public int Status { get; set; }
        public double PermanentDiscount { get; set; }
    }
}