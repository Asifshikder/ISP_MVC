using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
 
namespace Project_ISP.Models
{
    public class BandwithResellerGivenItem
    {
        [Key]
        //[Column("BRGII")]
        public int BandwithResellerGivenItemID { get; set; }  
        //[Column("IN")]
        public string ItemName { get; set; }  
        //[Column("MU")]
        public string MeasureUnit { get; set; }  
        //[Column("PUCP")]
        public string PerUnitCommonPrice { get; set; }  
        //[Column("SS")]
        public int Status { get; set; }
        //[Column("CB")]
        public string CreatedBy { get; set; }
        //[Column("CD")]
        public DateTime? CreatedDate { get; set; }
        //[Column("UB")]
        public string UpdateBy { get; set; }
        //[Column("UD")]
        public DateTime? UpdateDate { get; set; }
    }
}