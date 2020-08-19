using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class CompanyVsPayerViewModel
    {
        public int PayerID { get; set; }
        public string PayerName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public bool UpdateCompanyVsPayer { get; set; }
    }
}