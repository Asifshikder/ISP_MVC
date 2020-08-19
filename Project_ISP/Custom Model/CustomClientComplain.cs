using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class CustomComplain
    {
        public int ComplainID { get; set; }
        public int ClientDetailsID { get; set; }
        public int TransactionID { get; set; }
        public string TicketNo { get; set; }
        public string RunningMonthSerial { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string Zone { get; set; }
        public string Contact { get; set; }
        public string ComplainType { get; set; }
        public string Complain { get; set; }
        public string WhichOrWhere { get; set; }
        public string Time { get; set; }
        public string AssignTo { get; set; }
        public string ComplainOpenBy { get; set; }
        public string Status { get; set; }
        public string SignalInString { get; set; }
        public bool DeleteUpdate { get; set; }
        public string ButtonList { get; set; }
        public bool IsPriorityClient { get; set; }
        public string ClientLoginName { get; set; }
        public string LineStatusActiveDate { get; set; }
        public int TotalComplainCount { get; set; }
        public string totalelementsgiven { get; set; }
    }
}