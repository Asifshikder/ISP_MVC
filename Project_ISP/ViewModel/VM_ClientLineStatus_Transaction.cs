using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_ClientLineStatus_Transaction
    {
        public List<ClientLineStatus> lstClientLineStatus { get; set; }
        public List<Transaction> lstTransaction { get; set; }
    }
}