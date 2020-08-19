using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("17")]
    public class Mikrotik
    {
        [Key]
        //[Column("AN")]
        public int MikrotikID { get; set; }
        //[Column("AM")]
        public string MikName { get; set; }
        //public int IPPoolID { get; set; }
        //public virtual IPPool IPPool { get; set; }
        //[Column("AC")]
        public string RealIP { get; set; }
        //[Column("AH")]
        public string MikUserName { get; set; }
        //[Column("AAAA")]
        public string MikPassword { get; set; }
        //[Column("AQ")]
        public int APIPort { get; set; }
        //[Column("AEE")]
        public int WebPort { get; set; }
        //[Column("AF")]
        public int CreatedBy { get; set; }
        //[Column("DA")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AS")]
        public int UpdateBy { get; set; }
        //[Column("A")]
        public DateTime? UpdateDate { get; set; }

    }
}