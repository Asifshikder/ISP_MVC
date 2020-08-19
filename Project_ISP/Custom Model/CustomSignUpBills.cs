using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class CustomSignUpBills
    {
        public int TransactionID { get; set; }
        public int ClientDetailsID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string ZoneName { get; set; }
        public string PackageName { get; set; }
        public string PackagePrice { get; set; }
        public string SignUpFee { get; set; }
        public DateTime PaymentDate { get; set; }
        public string RemarksNo { get; set; }
        public string CreateRemarks { get; set; }
        public string ReceiptNo { get; set; }
        public string FeeForThisMonth { get; set; }
        public bool IsPriorityClient { get; set; }

        public string Reference { get; set; }
        public string GivenDetails { get; set; }
        public string GivenCableDetails { get; set; }
        public string GivenItemsDetails { get; set; }
        public string CreateBy { get; set; }
        public string InstallBy { get; set; }
        public string ItemInstalledEmployeeNameList { get; set; }


        public string ClientLoginName { get; set; }

        public string LineStatusActiveDate { get; set; }
    }
}