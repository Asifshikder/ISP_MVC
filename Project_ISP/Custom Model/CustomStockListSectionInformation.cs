using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class CustomStockListSectionInformation
    {
        public int StockDetailsID { get; set; }
        public int SectionID { get; set; }
        public int ProductStatusID { get; set; }
        public int ClientDetailsID { get; set; }
        public int TransactionID { get; set; }
        public int DistributionID { get; set; }
        public string ClientName { get; set; }
        public string EmployeeName { get; set; }
        public string PopName { get; set; }
        public string BoxName { get; set; }
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public string Serial { get; set; }
        public string SectionName { get; set; }
        public string ProductStatusName { get; set; }
        public bool ChangeSectionPermission { get; set; }
        public string WarrentyProduct { get; set; }
         public bool IsPriorityClient { get; set; }
        public string ClientLoginName { get; set; }
        public string LineStatusActiveDate { get; set; }
        public string Remarks { get; set; }
    }
}