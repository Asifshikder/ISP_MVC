using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    public class CustomCableTypeOverview
    {
        public int CableTypeID { get; set; }
        public string CableTypeName { get; set; }
        public int TotalCableTypeCount { get; set; }
    }
}