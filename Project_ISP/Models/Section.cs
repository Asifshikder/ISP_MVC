using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("30")]
    public class Section
    {
        [Key]
        //[Column("A")]
        public int SectionID { get; set; }
        //[Column("AQ")]
        public string SectionName { get; set; }
        //[Column("AW")]
        public string CreatedBy { get; set; }
        //[Column("AE")]
        public DateTime? CreatedDate { get; set; }
        //[Column("ART")]
        public string UpdateBy { get; set; }
        //[Column("AT")]
        public DateTime? UpdateDate { get; set; }
    }
}