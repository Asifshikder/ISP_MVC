using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    public class SMSReturnDetails
    {
        public string statusCode { get; set; }
        public string statusMsg { get; set; }
        public string destination { get; set; }
        public string message_id { get; set; }
    }
}