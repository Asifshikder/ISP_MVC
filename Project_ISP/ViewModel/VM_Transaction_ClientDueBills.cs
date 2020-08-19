using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_Transaction_ClientDueBills
    {
        public Transaction Transaction { get; set; }
        public ClientDueBills ClientDueBills { get; set; }
    }

    

    public class VM_Transaction_EmpTraLockUnlock_ClientDueBills
    {

        public string chkSMS { get; set; }
        public string chkPayBill { get; set; }
        public int TransactionID { get; set; }
        public bool Paid { get; set; }
        public int ClientDetailsID { get; set; }
        public string ClientName { get; set; }
        public string ClientLoginName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ZoneName { get; set; }
        public int PackageID { get; set; }
        public string PackageName { get; set; }
        public double MonthlyFee { get; set; }
        public double FeeForThisMonth { get; set; }
        public double PaidAmount { get; set; }
        public double Discount { get; set; }
        public int PaymentStatus { get; set; }
        public double Due { get; set; }
        public int PaymentStatusFullyOrPartiallyOrNotPaid { get; set; }
        public string PaidBy { get; set; }
        public string CollectBy { get; set; }
        public string PaidTime { get; set; }
        public string RemarksNo { get; set; }
        public string ReceiptNo { get; set; }
        public int LineStatusID { get; set; }

        public int StatusThisMonthID {get;set; }
        public int StatusNextMonthID { get; set; }
        public string Employeename { get; set; }

        public bool IsPriorityClient { get; set; }
        public string LineStatusActiveDate { get; set; }
        public double PermanentDiscount { get; set; }
    }


    public class VM_Paid_History_Employee
    {
        
        public int TransactionID { get; set; }
        public string LoginID { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string ZoneName { get; set; }
        public string PackageName { get; set; }
        public double FeeForThisMonth { get; set; }
        public double PaidAmount { get; set; }
        public double Discount { get; set; }
        public string CollectBy { get; set; }
        public string CollectByName { get; set; }
        public string PaidTime { get; set; }
        public string RemarksNo { get; set; }
        public string ReceiptNo { get; set; }
        public string PaymentType { get; set; }
        public string PaymentBy { get; set; }
        public string TotalAmount { get; set; } 
        public string TotalAllAmount { get; set; }
        public string TotalActualAmount { get; set; }
        public string TotalDiscountAmount { get; set; }
        public string TotalAdvanceAmount { get; set; }
        public bool ShowDeleteButton { get; set; }
        public bool ShowAcceptButton { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class VM_Transaction_ClientDueBills_Temp
    {
        public Transaction Transaction { get; set; }
        public List<ClientDueBills> ClientDueBills { get; set; }
    }
}