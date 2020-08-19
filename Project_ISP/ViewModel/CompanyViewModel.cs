using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class CompanyViewModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyAddress { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string CompanyLogoPath { get; set; }
        public bool UpdateCompany { get; set; }
    }
}