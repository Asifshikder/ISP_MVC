using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP_ManagementSystemModel.Models;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_Zone_ClientDetails
    {
        public List<ClientDetails> lstClientDetails { get; set; }

        public List<Zone> lstZone { set; get; }
    }
}