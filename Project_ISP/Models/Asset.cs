using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISP_ManagementSystemModel.Models
{

    //[Table("ast")]
    public class Asset
    {
        [Key]
        //[Column("A")]

        public int AssetID { get; set; }
        public int AssetTypeID { get; set; }
        public virtual AssetType AssetType { get; set; }
        //[Column("AA3")]
        public string AssetName { get; set; }
        //[Column("AS")]
        public double AssetValue { get; set; }
        //[Column("AA1")]
        public DateTime PurchaseDate { get; set; }
        //[Column("AA2")]
        public string SerialNumber { get; set; }
        //[Column("AA31")]
        public DateTime? WarrentyStartDate { get; set; }
        //[Column("AS1")]
        public DateTime? WarrentyEndDate { get; set; }
        //[Column("AD")]
        public string CreatedBy { get; set; }
        //[Column("AF")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AG")]
        public string UpdateBy { get; set; }
        //[Column("AH")]
        public DateTime? UpdateDate { get; set; }
    }
}