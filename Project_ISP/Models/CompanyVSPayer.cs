using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    public class CompanyVSPayer
    {
        [Key]
        public int PayerID { get; set; }
        public string PayerName { get; set; }
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}