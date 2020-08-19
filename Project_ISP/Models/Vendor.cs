using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    public class Vendor
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string CompanyName { get; set; }
        public string VendorLogoName { get; set; }
        public byte[] VendorImageOriginalName{ get; set; }
        public string VendorImagePath { get; set; }
        public string VendorContactPerson { get; set; }
        public string VendorEmail { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int VendorTypeID { get; set; }
        public virtual VendorType VendorType { get; set; }
    }
}