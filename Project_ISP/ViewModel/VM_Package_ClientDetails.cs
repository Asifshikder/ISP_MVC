using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_Package_ClientDetails
    {
        public List<ClientDetails> lstClientDetails { get; set; }
        public List<Package> lstPackage { get; set; }
    }
}