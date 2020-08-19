using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISP_ManagementSystemModel.Models;

namespace ISP_ManagementSystemModel.ViewModel
{
    public class VM_Form_Action_UserRight
    {
        public FormNameForAuth FormNameForAuth { get; set; }
        public List<ActionNameAuthentication> ActionNameAuthentication { get; set; }
    }
}