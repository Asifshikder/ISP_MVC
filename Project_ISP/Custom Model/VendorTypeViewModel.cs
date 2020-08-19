using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Custom_Model
{
    public class VendorTypeViewModel
    {
        public int VendorTypeID { get; set; }
        public string VendorTypeName { get; set; }
        public int TableStatusID { get; set; }
        public bool UpdateVendorType { get; set; }
    }
}