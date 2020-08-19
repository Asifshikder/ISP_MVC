using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class CustomCableUsedInformation
    {
        public int CableDistributionID { get; set; }
        public int ClientDetailsID { get; set; }
        public int TransactionID { get; set; }
        public string CableTypeName { get; set; }
        public string CableBoxName { get; set; }
        public string AmountOfCableUsed { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public string AssignEmployeeName { get; set; }
        public string EmployeeTakenCable { get; set; }
        public string cableStatus { get; set; }
        public bool ChangeStatus { get; set; }
        public bool IsPriorityClient { get; set; }
        public string ClientLoginName { get; set; }
        public string LineStatusActiveDate { get; set; }
        public string Remarks { get; set; }
        public string CableFinishMinusView { get; set; }
    }
}