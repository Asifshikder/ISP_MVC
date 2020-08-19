using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("24")]
    public class Pop
    {
        [Key]
        //[Column("YYA")]
        public int PopID { get; set; }
        //[Column("AT")]
        public string PopName { get; set; }
        //[Column("AR")]
        public string PopLocation { get; set; }

        public string LatitudeLongitude { get; set; }
        //[Column("AE")]
        public string CreatedBy { get; set; }
        //[Column("AW")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AQ")]
        public string UpdateBy { get; set; }
        //[Column("A")]
        public DateTime? UpdateDate { get; set; }
    }
}