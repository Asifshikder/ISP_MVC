using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel.CustomClass
{
    public class VM_ClientDetails
    {
            public int ClientDetailsID { get; set; }
            public string Name { get; set; }
            public string LoginName { get; set; }
            public string PackageName { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string ZoneName { get; set; }
            public string ContactNumber { get; set; }
            public bool IsPriorityClient { get; set; }
            public string ProfileUpdatePercentage { get; set; } 
            public double PermanentDiscount { get; set; } 
    }
}