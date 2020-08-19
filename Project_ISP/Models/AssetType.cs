using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{
    //[Table("at")]
    public class AssetType
    {
        [Key]
        //[Column("A")]
        public int AssetTypeID { get; set; }
        //[Column("AA")]
        public string AssetTypeName { get; set; }
        //[Column("AS")]
        public string CreatedBy { get; set; }
        //[Column("AD")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AF")]
        public string UpdateBy { get; set; }
        //[Column("AG")]
        public DateTime? UpdateDate { get; set; }
        //[Column("AH")]
        public bool Status { get; set; }
    }
}