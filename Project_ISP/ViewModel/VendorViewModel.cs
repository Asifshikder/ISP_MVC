using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class VendorViewModel
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string CompanyName { get; set; }
        public string VendorLogInName { get; set; }
        public byte[] VendorImageOriginalName { get; set; }
        public string VendorImagePath { get; set; }
        public string VendorContactPerson { get; set; }
        public string VendorEmail { get; set; }
        public int Status { get; set; }
        public int VendorTypeID { get; set; }
        public string VendorTypeName { get; set; }
        public bool VendorUpdate { get; set; }
    }
}