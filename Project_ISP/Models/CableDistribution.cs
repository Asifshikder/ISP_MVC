using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;
namespace ISP_ManagementSystemModel.Models
{
    //[Table("i")]
    public class CableDistribution
    {
        [Key]
        //[Column("AH")]
        public int CableDistributionID { get; set; }
        //[Column("ADDD")]
        public int ?ClientDetailsID { get; set; }
        public virtual ClientDetails ClientDetails { get; set; }

        //this employee id is who is assigned for set up those things directly not for client
        //[Column("AX")]
        public int ?EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
        //this is the employee who taken cable for some pourpous but this is not the Assigned Employee
        //[Column("AZ")]
        public int? CableForEmployeeID { get; set; }
        //[Column("AG")]
        public int CableStockID { get; set; }
        public virtual CableStock CableStock { get; set; }
        //[Column("AF")]
        public int AmountOfCableUsed { get; set; }
        //[Column("AD")]
        public string Purpose { get; set; }
        //[Column("AS")]
        public int CableAssignFromWhere { get; set; }
        //[Column("AA")]
        public int CableIndicatorStatus { get; set; }
        public string Remarks { get; set; }
        //[Column("AR")]
        public string CreatedBy { get; set; }
        //[Column("AE")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AW")]
        public string UpdateBy { get; set; }
        //[Column("AQ")]
        public DateTime? UpdateDate { get; set; }
    }
}