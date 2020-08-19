using ISP_ManagementSystemModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.Models
{
    public class Head
    {
        public int HeadID { get; set; }
        public string HeadeName { get; set; }
        public int HeadTypeID { get; set; }
        public int? ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int Status { get; set; }

    }
}