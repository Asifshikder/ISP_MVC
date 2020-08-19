using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

using System.ComponentModel.DataAnnotations.Schema;namespace ISP_ManagementSystemModel.Models
{
    //[Table("43")]
    public class Zone
    {
        [Key]
        //[Column("KA")]
        public int ZoneID { get; set; }
        //[Column("QA")]
        public string ZoneName { get; set; }
        public int? ResellerID { get; set; }
        public virtual Reseller Reseller { get; set; }
        //[Column("WA")]
        public string CreatedBy { get; set; }
        //[Column("AS")]
        public DateTime? CreatedDate { get; set; }
        //[Column("AA")]
        public string UpdateBy { get; set; }
        //[Column("A")]
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<ClientDetails> ClientDetails { get; set; }
    }
}