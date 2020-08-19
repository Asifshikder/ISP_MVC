using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("2")]
    public class DirectProductSectionChangeFromWorkingToOthers
    {
        [Key]
        //[Column("A")]
        public int DirectProductSectionChangeFromWorkingToOthersID { get;set; }
        //[Column("AZX")]
        public string ClientName { get; set; }
        //[Column("AQQ")]
        public string TakenEmployee { get; set; }
        //[Column("AF")]
        public int StockDetailsID { get; set; }
        public virtual  StockDetails StockDetails { get; set; }
        //[Column("AS")]
        public int FromSection { get; set; }
        //[Column("AE")]
        public int ToSection { get; set; }
        //[Column("AW")]
        public string WhoChanged { get; set; }
        //[Column("AQ")]
        public DateTime ChangeDateTime { get; set; }
    }
}