using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("12")]
    public class IPPool
    {
        [Key]
        //[Column("A")]
        public int IPPoolID { get; set; }
        //[Column("AQ")]
        public string PoolName { get; set; }
        //[Column("AW")]
        public string StartRange { get; set; }
        //[Column("AE")]
        public string EndRange { get; set; }
        //[Column("AR")]
        public int CreatedBy { get; set; }
        //[Column("AS")]
        public DateTime? CreatedDate { get; set; }
        //[Column("ADD")]
        public int UpdateBy { get; set; }
        //[Column("AF")]
        public DateTime? UpdateDate { get; set; }
    }
}